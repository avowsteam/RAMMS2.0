using Microsoft.Extensions.DependencyInjection;
using RAMMS.Business.ServiceProvider;
using RAMMS.Business.ServiceProvider.Interfaces;
using RAMMS.Business.ServiceProvider.Services;
using RAMMS.Domain.Models;
using RAMMS.Repository;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using Microsoft.Extensions.Configuration;
using System;
using RAMMS.DTO;
using AutoMapper;
using Serilog;
using Microsoft.Extensions.Logging;
using RAMMS.Common.Extensions;
using RAMMS.Common;

namespace RAMMS.Root.CustomInjection
{
    //Composition Root Injection
    public static class AppRoot
    {
        public static void InjectAppDependencies(this IServiceCollection services)
        {
            InjectServiceDependencies(services);

            InjectRepositoryDependencies(services);

            InjectProviderDependicies(services);

            //var logger = new LoggerConfiguration()
            //   .Enrich.FromLogContext()
            //   .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            //   .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
            //   .WriteTo.Console()
            //   .CreateLogger();

            //services.AddLogging(b =>
            //{
            //    b.AddSerilog(logger, true);
            //});
            //services.AddSingleton(typeof(Serilog.ILogger), logger);
        }

        private static void InjectServiceDependencies(IServiceCollection services)
        {
            services.AddTransient<IFormAService, FormAService>();
            services.AddTransient<IFormAImageService, FormAImageService>();
            services.AddTransient<IFormJImageService, FormJImageService>();
            services.AddTransient<IFormHImageService, FormHImageService>();
            services.AddTransient<IFormABO, FormABO>();
            services.AddTransient<IBridgeBO, BridgeBO>();
            services.AddTransient<IAllAssetBO, AllAssetBO>();
            services.AddTransient<IUserBO, UserBO>();
            services.AddTransient<IDDLookupBO, DDLookupBO>();
            services.AddTransient<IAssetsService, AssetsService>();
            services.AddTransient<IDDLookUpService, DDLookUpService>();
            services.AddTransient<IRoadMasterService, RoadMasterService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IFormXService, FormXService>();
            services.AddTransient<IFormHService, FormHService>();
            services.AddTransient<IFormDService, FormDService>();
            services.AddTransient<IFormJServices, FormJService>();
            services.AddTransient<ISecurity, Security>();
            services.AddTransient<IFormN1Service, FormN1Service>();
            services.AddTransient<IFormN2Service, FormN2Service>();
            services.AddTransient<IFormQa2Service, FormQa2Service>();
            services.AddTransient<IFormS2Service, FormS2Service>();
            services.AddTransient<ILandingHomeService, LandingHomeService>();
            services.AddTransient<IReportGenerationService, ReportGenerationService>();
            services.AddTransient<IFormS1Service, FormS1Service>();
            services.AddTransient<IFormF2Service, FormF2Service>();
            services.AddTransient<IFormC1C2Service, FormC1C2Service>();
            services.AddTransient<IFormB1B2Service, FormB1B2Service>();
            services.AddTransient<IFormFDService, FormFDService>();
            services.AddTransient<IFormF4Service, FormF4Service>();
            services.AddTransient<IFormF5Service, FormF5Service>();
            services.AddTransient<IFormFCService, FormFCService>();
            services.AddTransient<IFormFSService, FormFSService>();
            services.AddTransient<IAdministrationService, AdministrationService>();
            services.AddTransient<IModuleGroupRightsService, ModuleGroupRightsService>();
            services.AddTransient<IAuditTransactionService, AuditTransactionService>();
            services.AddTransient<IAuditActionService, AuditActionService>();
            services.AddTransient<IProcessService, ProcessService>();
            services.AddTransient<IDivisionService, DivisionService>();
            services.AddTransient<IRMUService, RMUService>();
			services.AddTransient<ISectionService, SectionService>();
            services.AddTransient<IRoadService, RoadService>();
            services.AddTransient<IFormW1Service, FormW1Service>();
            services.AddTransient<IFormW2Service, FormW2Service>();
            services.AddTransient<IFormWCService, FormWCService>();
            services.AddTransient<IFormWGService, FormWGService>();
            services.AddTransient<IFormWDService, FormWDService>();
            services.AddTransient<IFormWNService, FormWNService>();
            services.AddTransient<IFormV1Service, FormV1Service>();
            services.AddTransient<IFormV2Service, FormV2Service>();
            services.AddTransient<IFormQa1Service, FormQa1Service>();
            services.AddTransient<IFormG1G2Service, FormG1G2Service>();
            services.AddTransient<IFormF3Service, FormF3Service>();
            services.AddTransient<IFormR1R2Service, FormR1R2Service>();
            services.AddTransient<IFormF1Service, FormF1Service>();
            services.AddTransient<IFormTService, FormTService>();
            services.AddTransient<IFormMService, FormMService>();
            services.AddTransient<IFormB9Service, FormB9Service>();
            services.AddTransient<IFormB10Service, FormB10Service>();
            services.AddTransient<IFormB7Service, FormB7Service>();
            services.AddTransient<IFormB11Service, FormB11Service>();
            services.AddTransient<IFormB8Service, FormB8Service>();
            services.AddTransient<IFormB14Service, FormB14Service>();
            services.AddTransient<IFormB13Service, FormB13Service>();
        }

        private static void InjectProviderDependicies(IServiceCollection services)
        {
            //services.AddTransient<IFormAProvider, FormAProvider>();
            //services.AddTransient<IFormXProvider, FormXProvider>();
            //services.AddTransient<IFormDProvider, FormDProvider>();
            //services.AddTransient<IUserProvider, UserProvider>();
            //services.AddTransient<IDDLookupProvider, DDLookupProvider>();
            ////services.AddTransient<IBridgeProvider, BridgeProvider>();
            //services.AddTransient<IAllAssetProvider, AllAssetProvider>();
            //services.AddTransient<IFormN1Provider, FormN1Provider>();
        }

        private static void InjectRepositoryDependencies(IServiceCollection services)
        {
            services.AddTransient<IRepositoryUnit, RepositoryUnit>();
            services.AddTransient<IFormARepository, FormARepository>();
            services.AddTransient<IFormXRepository, FormXRepository>();
            services.AddTransient<IFormDRepository, FormDRepository>();
            services.AddTransient<IFormADtlRepository, FormADtlRepository>();
            services.AddTransient<IFormAImgRepository, FormAImgRepository>();
            services.AddTransient<IDDLookUpRepository, DDLookupRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoadMasterRepository, RoadmasterRepository>();
            services.AddTransient<IAssetRepository, AssetRepository>();
            services.AddTransient<IRmAssetImgRepository, RmAssetImgDtlRepository>();
            services.AddTransient<IFormHRepository, FormHRepository>();
            services.AddTransient<IFormJRepository, FormJRepository>();
            services.AddTransient<IFormN1Repository, FormN1Repository>();
            services.AddTransient<IFormN2Repository, FormN2Repository>();
            services.AddTransient<IFormQa2Repository, FormQa2Repository>();
            services.AddTransient<IFormQa2DtlRepository, FormQa2DtlRepository>();
            services.AddTransient<IFieldRightsRepository, FieldRightsRepository>();
            services.AddTransient<IModuleRightsRepository, ModuleRightsRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IModuleRepository, ModuleRepository>();
            services.AddTransient<IFormS1Repository, FormS1Repository>();
            services.AddTransient<IFormC1C2Repository, FormC1C2Repository>();
            services.AddTransient<IFormFDRepository, FormFDRepository>();
            services.AddTransient<IFormF4Repository, FormF4Repository>();
            services.AddTransient<IFormF5Repository, FormF5Repository>();
            services.AddTransient<IFormFCRepository, FormFCRepository>();
            services.AddTransient<IAdministratorRepository, AdministratorRepository>();
            services.AddTransient<IDivRmuSectionRepository, DivRmuSectionRepository>();
            services.AddTransient<IFormW2Repository, FormW2Repository>();
            services.AddTransient<IFormW2FcemRepository, FormW2FcemRepository>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddTransient<IFormW1Repository, FormW1Repository>();
            services.AddTransient<IFormWCRepository, FormWCRepository>();
            services.AddTransient<IFormWGRepository, FormWGRepository>();
            services.AddTransient<IFormWDRepository, FormWDRepository>();
            services.AddTransient<IFormWNRepository, FormWNRepository>();
            services.AddTransient<IModuleFormRightsRepository, ModuleFormRightsRepository>();
            services.AddTransient<IFormV1Repository, FormV1Repository>();
            services.AddTransient<IFormV2Repository, FormV2Repository>();
            services.AddTransient<IFormV2EquipmentRepository, FormV2EquipmentRepository>();
            services.AddTransient<IFormV2MaterialRepository, FormV2MaterialRepository>();
            services.AddTransient<IFormV2LabourRepository, FormV2LabourRepository>();
            services.AddTransient<IFormQa1Repository, FormQa1Repository>();
            services.AddTransient<IFormG1Repository, FormG1Repository>();
            services.AddTransient<IFormF3Repository, FormF3Repository>();
            services.AddTransient<IFormR1Repository, FormR1R2Repository>();
            services.AddTransient<IFormF1Repository, FormF1Repository>();
            services.AddTransient<IFormTRepository, FormTRepository>();
            services.AddTransient<IFormMRepository, FormMRepository>();
            services.AddTransient<IFormB9Repository, FormB9Repository>();
            services.AddTransient<IFormB10Repository, FormB10Repository>();
            services.AddTransient<IFormB7Repository, FormB7Repository>();
            services.AddTransient<IFormB11Repository, FormB11Repository>();
            services.AddTransient<IFormB8Repository, FormB8Repository>();
            services.AddTransient<IFormB14Repository, FormB14Repository>();
            services.AddTransient<IFormB13Repository, FormB13Repository>();
        }

        //public static void InjectAppDependencies(IServiceCollection services)
        //{
        //    services.AddTransient<IUserProvider, UserProvider>();
        //    services.AddTransient<IUserRepository, UserRepository<RmUsers>>();
        //    services.AddTransient<IUserBO, UserBO>();
        //    services.AddTransient<IDDLookupProvider, DDLookupProvider>();
        //    services.AddTransient<IDDLookupRepository, DDLookupRepository<RmDdLookup>>();
        //    services.AddTransient<IDDLookupBO, DDLookupBO>();

        //    services.AddTransient<IRepositoryUnit, RepositoryUnit>();
        //    services.AddTransient<IFormAProvider, FormAProvider>();
        //    services.AddTransient<IFormARepository, FormARepository>();
        //    services.AddTransient<IFormADtlRepository, FormADtlRepository>();
        //    services.AddTransient<IFormAImgRepository, FormAImgRepository<RmFormaImageDtl>>();

        //    services.AddTransient<IFormAService, FormAService>();
        //    services.AddTransient<IFormABO, FormABO>();
        //    services.AddTransient<IBridgeProvider, BridgeProvider>();
        //    services.AddTransient<IRoadmasterRepository, RoadmasterRepository<RmRoadMaster>>();
        //    services.AddTransient<IBridgeBO, BridgeBO>();

        //    services.AddTransient<IAllAssetBO, AllAssetBO>();
        //    services.AddTransient<IAllAssetProvider, AllAssetProvider>();
        //    services.AddTransient<IAllAssetRepository, AllAssetRepository<RmAllassetInventory>>();
        //}
    }
}
