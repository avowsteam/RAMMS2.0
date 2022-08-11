using RAMMS.Domain.Models;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RAMMS.Repository
{
    public class RepositoryUnit : IRepositoryUnit
    {
        public RAMMSContext _context { get; private set; }
        private FormARepository _formARepository;
        private FormDRepository _formDRepository;
        private DDLookupRepository _dDLookUpRepository;
        private AssetRepository _allAssetRepository;
        private RoadmasterRepository _roadmasterRepository;
        private UserRepository _userRepository;
        private RmAssetImgDtlRepository _rmAssetImgDtlRepository;
        private FormXRepository _formXRepository;
        private FormHRepository _formHRepository;
        private FormAImgRepository _formAImgRepository;
        private FormJRepository _formJRepositoryl;
        private FormDLabourRepository _formDLabourRepository;
        private FormDEduipmentRepository _formDEquipmentRepository;
        private FormDMaterialRepository _formDMaterialRepository;
        private FormDDtlRepository _formDDtlRepository;
        private FormJImgRepository _formJImgRepository;
        private FormN1Repository _formN1Repository;
        private FormN2Repository _formN2Repository;
        private FormQa2Repository _formQa2Repository;
        private FormQa2DtlRepository _formQa2DtlRepository;
        private FormHImgRepository _formHImgRepository;
        private FormS2Repository _formS2Repository;
        private FormS2DetailRepository _formS2DetailRepository;
        private CalendarRepository calendarRepository;
        private FormS2QuarterDtlRepository _forms2quarterRepository;
        private FormADtlRepository _formAdtlRepository;
        private FormF2Repository _formF2Repository;
        private FormF2DetailRepository _formF2DetailRepository;
        private FormB1B2HeaderRepository _formB1B2HeaderRepository;
        private FormB1B2DetailRepository _formB1B2DetailRepository;
        
        private IFormB1B2ImgRepository _formB1B2ImgRepository;
        private IFormS1Repository _formS1Repository;
        private IFormFDRepository _formFDRepository;
        private IFormF4Repository _formF4Repository;
        private IFormF5Repository _formF5Repository;
        private IFormFSHeaderRepository _formFsHeaderRepository;
        private IFormFSDetailRepository _formFsDetailRepository;
        private IFormC1C2Repository _formC1C2Repository;
        private IDivRmuSectionRepository _divRmuSectionRepository;
        private FormW2Repository _formW2Repository;
        private FormW1Repository _formW1Repository;
		private ISectionRepository sectionRepository;
        private IRoadRepository roadRepository;
        private FormW2FcemRepository _formW2FcemRepository;
        private FormWCRepository _formWCRepository;
        private FormWGRepository _formWGRepository;
        private FormWDRepository _formWDRepository;
        private FormWNRepository _formWNRepository;
        private ModuleFormRightsRepository _moduleFormRightsRepository;
        private FormV1Repository _formV1Repository;

        private FormV2Repository _formV2Repository;
        private FormV2LabourRepository _formV2LabourRepository;
        private FormV2EquipmentRepository _formV2EquipmentRepository;
        private FormV2MaterialRepository _formV2MaterialRepository;
        private FormQa1Repository _formQa1Repository;
        private FormF3Repository _formF3Repository;
        private FormF1Repository _formF1Repository;
        private FormTRepository _formTRepository;
        private FormB9Repository _formB9Repository;
        private FormB10Repository _formB10Repository;
        private FormB7Repository _formB7Repository;

        private FormS2DayScheduleRepository _formS2DayScheduleRepository;

        private FormG1Repository _formG1Repository;

        public FormG1Repository FormG1Repository => _formG1Repository = _formG1Repository ?? new FormG1Repository(_context);

        private FormR1R2Repository _formR1Repository;

        public FormR1R2Repository FormR1Repository => _formR1Repository = _formR1Repository ?? new FormR1R2Repository(_context);

        public FormS2DayScheduleRepository FormS2DayScheduleRepository => _formS2DayScheduleRepository = _formS2DayScheduleRepository ?? new FormS2DayScheduleRepository(_context);

        public FormQa1Repository FormQa1Repository => _formQa1Repository = _formQa1Repository ?? new FormQa1Repository(_context);

        public FormF3Repository FormF3Repository => _formF3Repository = _formF3Repository ?? new FormF3Repository(_context);

        public FormF1Repository FormF1Repository => _formF1Repository = _formF1Repository ?? new FormF1Repository(_context);

        public FormTRepository FormTRepository => _formTRepository = _formTRepository ?? new FormTRepository(_context);

        public FormB9Repository FormB9Repository => _formB9Repository = _formB9Repository ?? new FormB9Repository(_context);
        public FormB10Repository FormB10Repository => _formB10Repository = _formB10Repository ?? new FormB10Repository(_context);

        public FormB7Repository FormB7Repository => _formB7Repository = _formB7Repository ?? new FormB7Repository(_context);

        public FormV1Repository FormV1Repository => _formV1Repository = _formV1Repository ?? new FormV1Repository(_context);
        public FormV2Repository FormV2Repository => _formV2Repository = _formV2Repository ?? new FormV2Repository(_context);
        public FormV2LabourRepository FormV2LabourRepository => _formV2LabourRepository = _formV2LabourRepository ?? new FormV2LabourRepository(_context);

        public FormV2EquipmentRepository FormV2EquipmentRepository => _formV2EquipmentRepository = _formV2EquipmentRepository ?? new FormV2EquipmentRepository(_context);

        public FormV2MaterialRepository FormV2MaterialRepository => _formV2MaterialRepository = _formV2MaterialRepository ?? new FormV2MaterialRepository(_context);
        public ModuleFormRightsRepository ModuleFormRightsRepository => _moduleFormRightsRepository = _moduleFormRightsRepository ?? new ModuleFormRightsRepository(_context);
        public FormWNRepository FormWNRepository => _formWNRepository = _formWNRepository ?? new FormWNRepository(_context);
        public FormWDRepository FormWDRepository => _formWDRepository = _formWDRepository ?? new FormWDRepository(_context);
        public FormW1Repository FormW1Repository => _formW1Repository= _formW1Repository ?? new FormW1Repository(_context);

        public FormW2FcemRepository FormW2FcemRepository => _formW2FcemRepository ?? new FormW2FcemRepository(_context);

        public FormW2Repository FormW2Repository => _formW2Repository = _formW2Repository ?? new FormW2Repository(_context);

        public FormWCRepository FormWCRepository => _formWCRepository = _formWCRepository ?? new FormWCRepository(_context);

        public FormWGRepository FormWGRepository => _formWGRepository = _formWGRepository ?? new FormWGRepository(_context);


        public IFormB1B2HeaderRepository FormB1B2HeaderRepository => _formB1B2HeaderRepository = _formB1B2HeaderRepository ?? new FormB1B2HeaderRepository(_context);
        public IFormB1B2DetailRepository FormB1B2DetailRepository => _formB1B2DetailRepository = _formB1B2DetailRepository ?? new FormB1B2DetailRepository(_context);
        public IFormB1B2ImgRepository FormB1B2ImgRepository => _formB1B2ImgRepository = _formB1B2ImgRepository ?? new FormB1B2ImageRepository(_context);
        public IFormFSHeaderRepository FormFSHeaderRepository => _formFsHeaderRepository = _formFsHeaderRepository ?? new FormFSHeaderRepository(_context);
        public IFormFSDetailRepository FormFSDetailRepository => _formFsDetailRepository = _formFsDetailRepository ?? new FormFSDetailRepository(_context);
        public DDLookupRepository DDLookUpRepository => _dDLookUpRepository = _dDLookUpRepository ?? new DDLookupRepository(_context);
        public FormARepository FormARepository => _formARepository = _formARepository ?? new FormARepository(_context);

        public FormXRepository FormXRepository => _formXRepository = _formXRepository ?? new FormXRepository(_context);

        public FormDRepository FormDRepository => _formDRepository = _formDRepository ?? new FormDRepository(_context);

        public FormN1Repository FormN1Repository => _formN1Repository = _formN1Repository ?? new FormN1Repository(_context);

        public FormN2Repository FormN2Repository => _formN2Repository = _formN2Repository ?? new FormN2Repository(_context);

        public FormQa2Repository FormQa2Repository => _formQa2Repository = _formQa2Repository ?? new FormQa2Repository(_context);

        public FormQa2DtlRepository FormQa2DtlRepository => _formQa2DtlRepository = _formQa2DtlRepository ?? new FormQa2DtlRepository(_context);

        public FormDLabourRepository FormDLabourRepository => _formDLabourRepository = _formDLabourRepository ?? new FormDLabourRepository(_context);

        //public FormDEduipmentRepository FormDEduipmentRepository => _formDEquipmentRepository = _formDEquipmentRepository ?? new FormDEduipmentRepository(_context);
        public FormDMaterialRepository FormDMaterialRepository => _formDMaterialRepository = _formDMaterialRepository ?? new FormDMaterialRepository(_context);
        public FormDDtlRepository FormDDtlRepository => _formDDtlRepository = _formDDtlRepository ?? new FormDDtlRepository(_context);
        public AssetRepository AllAssetRepository => _allAssetRepository = _allAssetRepository ?? new AssetRepository(_context);
        public RoadmasterRepository RoadmasterRepository => _roadmasterRepository = _roadmasterRepository ?? new RoadmasterRepository(_context);
        public UserRepository UserRepository => _userRepository = _userRepository ?? new UserRepository(_context);
        public RmAssetImgDtlRepository RmAssetImgDtlRepository => _rmAssetImgDtlRepository = _rmAssetImgDtlRepository ?? new RmAssetImgDtlRepository(_context);
        public FormHRepository FormHRepository => _formHRepository = _formHRepository ?? new FormHRepository(_context);
        public IFormAImgRepository FormAImgDtlRepository => _formAImgRepository = _formAImgRepository ?? new FormAImgRepository(_context);

        public FormDEduipmentRepository FormDEquipmentRepository => _formDEquipmentRepository = _formDEquipmentRepository ?? new FormDEduipmentRepository(_context);

        public FormADtlRepository FormADtlRepository => _formAdtlRepository = _formAdtlRepository ?? new FormADtlRepository(_context);

        public FormJRepository FormJRepository => _formJRepositoryl = _formJRepositoryl ?? new FormJRepository(_context);
        public FormS2Repository FormS2Repository => _formS2Repository = _formS2Repository ?? new FormS2Repository(_context);
        public FormS2DetailRepository FormS2DetailRepository => _formS2DetailRepository = _formS2DetailRepository ?? new FormS2DetailRepository(_context);
        public IFormJImgRepository FormJImgDtlRepository => _formJImgRepository = _formJImgRepository ?? new FormJImgRepository(_context);
        public CalendarRepository CalendarRepository => calendarRepository = calendarRepository ?? new CalendarRepository(_context);

        public FormHImgRepository FormHImgRepository => _formHImgRepository = _formHImgRepository ?? new FormHImgRepository(_context);
        public FormS2QuarterDtlRepository FormS2QuarterDtlRepository => _forms2quarterRepository = _forms2quarterRepository ?? new FormS2QuarterDtlRepository(_context);
        public IFormF2Repository FormF2Repository => _formF2Repository = _formF2Repository ?? new FormF2Repository(_context);
        public IFormF2DetailRepository FormF2DetailRepository => _formF2DetailRepository = _formF2DetailRepository ?? new FormF2DetailRepository(_context);

        public IFormS1Repository formS1Repository => _formS1Repository = _formS1Repository ?? new FormS1Repository(_context);

        public IFormFDRepository FormFDRepository => _formFDRepository = _formFDRepository ?? new FormFDRepository(_context);
        public IFormF4Repository formF4Repository => _formF4Repository = _formF4Repository ?? new FormF4Repository(_context);
        public IFormF5Repository formF5Repository => _formF5Repository = _formF5Repository ?? new FormF5Repository(_context);
        public IFormC1C2Repository formC1C2Repository => _formC1C2Repository = _formC1C2Repository ?? new FormC1C2Repository(_context);
        private IModuleGroupRepository formModuleGroupRepository;
        public IModuleGroupRepository ModuleGroupRepository => formModuleGroupRepository = formModuleGroupRepository ?? new ModuleGroupRepository(_context);

        private IAuditActionRepository auditActionRepository;
        public IAuditActionRepository AuditActionRepository => auditActionRepository = auditActionRepository ?? new AuditActionRepository(_context);

        private IAuditTransactionRepository auditTransactionRepository;
        public IAuditTransactionRepository AuditTransactionRepository => auditTransactionRepository = auditTransactionRepository ?? new AuditTransactionRepository(_context);
        private IDivisonRepository divisonRepository;
        public IDivisonRepository DivisonRepository => divisonRepository = divisonRepository ?? new DivisonRepository(_context);
        private IRMURepository rMURepository;
        public IRMURepository RMURepository => rMURepository = rMURepository ?? new RMURepository(_context);

        public IDivRmuSectionRepository DivRmuSectionRepository => _divRmuSectionRepository ?? new DivRmuSectionRepository(_context);

        public ISectionRepository SectionRepository => this.sectionRepository  ?? new SectionRepository(_context);

        public IRoadRepository RoadRepository => roadRepository  ?? new RoadRepository(_context);


        public RepositoryUnit(RAMMSContext context)
        {
            this._context = context;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            _context.Dispose();
        }
        public async Task RollbackAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
