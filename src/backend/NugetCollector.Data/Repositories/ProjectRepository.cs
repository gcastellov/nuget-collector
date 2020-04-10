using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NugetCollector.Data.Model;
using NugetCollector.Domain.Repositories;

namespace NugetCollector.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public void Save(IEnumerable<Domain.Project> projects)
        {
            using (var dbContext = new ProjectContext())
            {
                ClearDatabase(dbContext);

                foreach (var project in projects)
                {
                    var model = new Project
                    {
                        Id = project.Name
                    };

                    foreach (var reference in project.References)
                    {
                        var referenceModel = dbContext.References.Find(reference.Name, reference.Version)
                                             ?? new Reference
                                             {
                                                 Name = reference.Name,
                                                 Version = reference.Version
                                             };

                        model.ProjectReferences.Add(new ProjectReference
                        {
                            Id = Guid.NewGuid(),
                            Project = model,
                            Reference = referenceModel
                        });
                    }

                    dbContext.Projects.Add(model);
                }

                dbContext.SaveChanges();
            }
        }

        public IEnumerable<Domain.Project> GetProjects()
        {
            using (var dbContext = new ProjectContext())
            {
                return dbContext.Projects.Include(p => p.ProjectReferences)
                    .Select(p => new Domain.Project(p.Id)
                    {
                        References = p.ProjectReferences.Select(r =>
                            new Domain.Reference(r.Reference.Name, r.Reference.Version))
                    })
                    .ToArray();
            }
        }

        private void ClearDatabase(ProjectContext dbContext)
        {
            if (dbContext.Projects.Any())
            {
                object[] projectReferences = dbContext.Projects
                    .Include(p => p.ProjectReferences)
                    .SelectMany(r => r.ProjectReferences)
                    .Distinct()
                    .ToArray();

                dbContext.RemoveRange(projectReferences);
                dbContext.RemoveRange(dbContext.References);
                dbContext.RemoveRange(dbContext.Projects);
                dbContext.SaveChanges();
            }
        }
    }
}