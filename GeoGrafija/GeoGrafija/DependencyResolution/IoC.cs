

using Services;
using Model;
using Model.Interfaces;
using Model.Repositories;
using Services.Interfaces;
using StructureMap;

namespace GeoGrafija 
{

    public static class IoC 
    {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
                
                //Services
                x.For<IUserService>().Use<UserService>();
                x.For<IRolesService>().Use<RolesService>();
                x.For<ILocationService>().Use<LocationService>();
                x.For<ISearchService>().Use<SearchService>();
                x.For<IResourceService>().Use<ResourceService>();
                x.For<ITeacherQuizesService>().Use<TeacherQuizesService>();
                x.For<IStudnetQuizesService>().Use<StudentQuizesService>();
                x.For<IBasicCrudService>().Use<BasicCrudService>();
                x.For<ILocalizedMessagesService>().Use<LocalizedMessagesService>();
                
                //Repositories
                x.For<IUserRepository>().Use<UserRepository>();
                x.For<IRolesRepository>().Use<RolesRepository>();
                x.For<ILocationRepository>().Use<LocationsRepository>();
                x.For<IResourceRepository>().Use<ResourceRepository>();
                x.For<IResourceTypeRepository>().Use<ResourceTypeRepository>();
                x.For<IQuizRepository>().Use<QuizRepository>();
                x.For<IQuestionRepository>().Use<QuestionRepository>();
                x.For<IAnswerRepository>().Use<AnswerRepository>();
                x.For<IStudentQuizResultRepository>().Use<StudentQuizResultRepository>();
               
                //Database contexts
                x.For<IDbContext>().HttpContextScoped().Use<GeoGrafijaEntities>();
                x.SelectConstructor<GeoGrafijaEntities>(() => new GeoGrafijaEntities());
            
            });
            
            return ObjectFactory.Container;
        }
    }
}