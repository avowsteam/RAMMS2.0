﻿using System;
using System.Threading.Tasks;
using RAMMS.Domain.Models;
using RAMMS.DTO.RequestBO;
using RAMMS.DTO.Wrappers;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using RAMMS.DTO.ResponseBO;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using System.Reflection.Emit;
using DocumentFormat.OpenXml.Office2010.Word;
using RAMMS.DTO.Report;

namespace RAMMS.Repository
{
    public class FormFSDetailRepository : RepositoryBase<RmFormFsInsDtl>, IFormFSDetailRepository
    {
        public FormFSDetailRepository(RAMMSContext context) : base(context) { _context = context ?? throw new ArgumentNullException(nameof(context)); }
        public async Task<long> GetFilteredRecordCount(FilteredPagingDefinition<FormFSDetailRequestDTO> filterOptions)
        {
            return await (from s in _context.RmFormFsInsDtl where s.FsdFshPkRefNo == filterOptions.Filters.FshPkRefNo && s.FsdActiveYn == true select s).LongCountAsync();
        }
        public async Task<List<FormFSDetailRequestDTO>> GetFilteredRecordList(FilteredPagingDefinition<FormFSDetailRequestDTO> filterOptions)
        {
            var query = (from s in _context.RmFormFsInsDtl
                         where s.FsdFshPkRefNo == filterOptions.Filters.FshPkRefNo && s.FsdActiveYn == true
                         orderby s.FsdFeature
                         select s);
            var lst = await query.Skip(filterOptions.StartPageNo).Take(filterOptions.RecordsPerPage).ToListAsync();
            return lst.Select(s => new FormFSDetailRequestDTO
            {
                PkRefNo = s.FsdPkRefNo,
                FshPkRefNo = s.FsdFshPkRefNo,
                Feature = s.FsdFeature,
                GrpType = s.FsdGrpType,
                StrucCode = s.FsdStrucCode,
                Width = s.FsdWidth,
                Length = s.FsdLength,
                Condition1 = s.FsdCondition1,
                Condition2 = s.FsdCondition2,
                Condition3 = s.FsdCondition3,
                Needed = s.FsdNeeded,
                Unit = s.FsdUnit,
                Remarks = s.FsdRemarks,
                ModBy = s.FsdModBy,
                ModDt = s.FsdModDt,
                CrBy = s.FsdCrBy,
                CrDt = s.FsdCrDt,
                SubmitSts = s.FsdSubmitSts,
                ActiveYn = s.FsdActiveYn.Value,
                GroupCode = s.FsdGrpCode
            }).ToList();
        }

        public async Task<List<FormFSDetailRequestDTO>> GetRecordList(int headerId)
        {
            string[] grpCodes = new string[] { "ELM", "RS", "CLM", "CW" };
            var query = (from s in _context.RmFormFsInsDtl
                         where s.FsdFshPkRefNo == headerId
                         && s.FsdActiveYn == true
                         orderby s.FsdFeature
                         select s);
            var lst = await query.ToListAsync();
            var avgClmWidth = "0";
            //var queryHeader =  (from s in _context.RmFormFsInsHdr
            //             where s.FshPkRefNo == headerId
            //             && s.FshActiveYn == true
            //             orderby s.FshRoadCode
            //             select s.FshRoadCode).FirstOrDefault();
           //var queryHeaderAll =  _context.RmAllassetInventory.Where(x => x.AiRdCode == queryHeader && x.AiActiveYn == true
           // && grpCodes.Contains(x.AiAssetGrpCode)).OrderBy(x => x.AiAssetGrpCode);

            

 var queryHeaderAll = (from h in _context.RmFormFcInsHdr
                         join fsh in _context.RmFormFsInsHdr on h.FcihRoadCode equals fsh.FshRoadCode
                       where fsh.FshPkRefNo == headerId && h.FcihYearOfInsp == fsh.FshYearOfInsp
                  select new 
                  {
                    
                      AssetTypes = h.FcihAssetTypes
                  }).FirstOrDefault();
            if (!string.IsNullOrEmpty(queryHeaderAll.AssetTypes))
            {
                var AvgWidth = Common.Utility.JDeSerialize<FormAssetTypesDTO>(queryHeaderAll.AssetTypes ?? "");

              

                if (AvgWidth.ContainsKey("CLM"))
                {
                    var cw = AvgWidth["CLM"];
                    foreach (var c in cw)
                    {
                        if (c.ContainsValue("Paint"))
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgClmWidth = c["AvgWidth"];
                            }
                        }
                        else if (c.ContainsValue("Thermoplastic"))
                        {
                            if (c.ContainsKey("AvgWidth"))
                            {
                                avgClmWidth = c["AvgWidth"];
                            }
                        }

                    }
                }
            }




            return lst.Select(s => new FormFSDetailRequestDTO
            {
                
            PkRefNo = s.FsdPkRefNo,
                FshPkRefNo = s.FsdFshPkRefNo,
                Feature = s.FsdFeature,
                GrpType = s.FsdGrpType,
                StrucCode = s.FsdStrucCode,
                //Width = s.FsdWidth != null ? s.FsdWidth : queryHeaderAll.Where(d=>d.AiAssetGrpCode==s.FsdGrpCode && d.AiGrpType == s.FsdGrpType).Select(d=>d.AiWidth).FirstOrDefault(),
                Width = s.FsdWidth != null ? s.FsdWidth :Convert.ToDouble(avgClmWidth),
                Length = s.FsdLength,
                Condition1 = s.FsdCondition1,
                Condition2 = s.FsdCondition2,
                Condition3 = s.FsdCondition3,
                Needed = s.FsdNeeded,
                Unit = s.FsdUnit,
                Remarks = s.FsdRemarks,
                ModBy = s.FsdModBy,
                ModDt = s.FsdModDt,
                CrBy = s.FsdCrBy,
                CrDt = s.FsdCrDt,
                SubmitSts = s.FsdSubmitSts,
                ActiveYn = s.FsdActiveYn.Value,
                GroupCode = s.FsdGrpCode
            }).ToList();
        }

        private IQueryable<RmDdLookup> GetAsset()
        {
            return (from d in _context.RmDdLookup
                    where d.DdlActiveYn == true && d.DdlType == "Asset Type"
                    select d);
        }
        public int BulkInsert(List<RmFormFsInsDtl> details, int headerid)
        {
            foreach (var d in details)
            {
                d.FsdFshPkRefNo = headerid;
            }
            _context.RmFormFsInsDtl.AddRange(details);
            return _context.SaveChanges();
        }


        public List<RmFormFsInsDtl> GetDetailsforInsert(int headerid, int userid, RmFormFsInsHdr hdr)
        {

            var carriageway = GetAsset().Where(s => s.DdlTypeCode == "CW").ToList();
            List<RmFormFsInsDtl> lst = new List<RmFormFsInsDtl>();
            foreach (var cw in carriageway)
            {
                var obj = GetCarriageWayDetail(cw.DdlTypeDesc, cw.DdlTypeValue, userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);
            }
            var centerlinemarking = GetAsset().Where(s => s.DdlTypeCode == "CLM").ToList();
            foreach (var cw in centerlinemarking)
            {
                var obj = GetCenterLineMarkingDetail(cw.DdlTypeDesc, cw.DdlTypeValue, userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);
            }

            var Edgelinemarking = GetAsset().Where(s => s.DdlTypeCode == "ELM").ToList();
            foreach (var cw in Edgelinemarking)
            {
                var obj = GetEDGELineMarkingDetail(cw.DdlTypeDesc, cw.DdlTypeValue, "L", userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);

                var obj1 = GetEDGELineMarkingDetail(cw.DdlTypeDesc, cw.DdlTypeValue, "R", userid, headerid, hdr);
                if (obj1 != null)
                    lst.Add(obj1);
            }

            var ditch = GetAsset().Where(s => s.DdlTypeCode == "DI").ToList();
            foreach (var cw in ditch)
            {
                var obj = GetDitchDetail(cw.DdlTypeDesc, cw.DdlTypeValue, "L", userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);

                var obj1 = GetDitchDetail(cw.DdlTypeDesc, cw.DdlTypeValue, "R", userid, headerid, hdr);
                if (obj1 != null)
                    lst.Add(obj1);
            }

            var drain = GetAsset().Where(s => s.DdlTypeCode == "DR").ToList();
            foreach (var cw in drain)
            {
                var obj = GetDrainDetail(cw.DdlTypeDesc, cw.DdlTypeValue, "L", userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);

                var obj1 = GetDrainDetail(cw.DdlTypeDesc, cw.DdlTypeValue, "R", userid, headerid, hdr);
                if (obj1 != null)
                    lst.Add(obj1);
            }
            var shoulder = GetAsset().Where(s => s.DdlTypeCode == "SH").ToList();
            foreach (var cw in shoulder)
            {
                var obj = GetShoulderDetail(cw.DdlTypeDesc, cw.DdlTypeValue, "L", userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);

                var obj1 = GetShoulderDetail(cw.DdlTypeDesc, cw.DdlTypeValue, "R", userid, headerid, hdr);
                if (obj1 != null)
                    lst.Add(obj1);
            }

            var roadstuds = GetAsset().Where(s => s.DdlTypeCode == "RS").ToList();
            foreach (var cw in roadstuds)
            {
                var obj = GetRoadStudsDetail(cw.DdlTypeDesc, cw.DdlTypeCode, "L", userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);
                var obj1 = GetRoadStudsDetail(cw.DdlTypeDesc, cw.DdlTypeCode, "M", userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);
                var obj2 = GetRoadStudsDetail(cw.DdlTypeDesc, cw.DdlTypeCode, "R", userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);
            }
            
			var culverts = GetAsset().Where(s => s.DdlTypeCode == "CV").ToList();
            foreach (var cw in culverts)
            {
                var obj = GetCulvertDetail(cw.DdlTypeDesc, cw.DdlTypeValue, userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);
            }

            var bridges = GetAsset().Where(s => s.DdlTypeCode == "BR").ToList();
            foreach (var cw in bridges)
            {
                var obj = GetBridgeDetail(cw.DdlTypeDesc, cw.DdlTypeValue, userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);
            }

            var guarrail = GetAsset().Where(s => s.DdlTypeCode == "GR").ToList();
            foreach (var cw in guarrail)
            {
                var obj = GetGuardialDetail(cw.DdlTypeDesc, cw.DdlTypeValue, userid, headerid, hdr);
                if (obj != null)
                    lst.Add(obj);
            }
           
		   var  sign = this.GetAsset().Where(s => s.DdlTypeCode == "SG");
            foreach (var rmDdLookup in sign)
            {
                var signs = this.GetSigns(rmDdLookup.DdlTypeDesc, rmDdLookup.DdlTypeValue, userid, headerid, hdr);
                if (signs != null)
                    lst.Add(signs);
            }
            var retainigWalls = this.GetAsset().Where(s => s.DdlTypeCode == "RW");
            foreach (var rmDdLookup in retainigWalls)
            {
                var retainingWall = this.GetRetainingWall(rmDdLookup.DdlTypeDesc, rmDdLookup.DdlTypeValue, userid, headerid, hdr);
                if (retainingWall != null)
                    lst.Add(retainingWall);
            }
            return lst.Where(s => s != null).ToList();
        }


        public RmFormFsInsDtl GetCarriageWayDetail(string grptype, string StructureCode, int userid, int headerid, RmFormFsInsHdr hdr)
        {

            var count = (from o in _context.RmFormFcInsDtl
                         join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                         where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CW" && o.FcidActiveYn == true && h.FcihActiveYn == true
                         && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihRoadCode == hdr.FshRoadCode && h.FcihSubmitSts == true
                         select 1).Count();
            decimal condition1 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CW" && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                  && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 1
                                  select 1).Count();
            decimal condition2 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype
                                  && h.FcihYearOfInsp == hdr.FshYearOfInsp && o.FcidAiAssetGrpCode == "CW" && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                  && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 2
                                  select 1).Count();
            decimal condition3 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype
                                  && h.FcihYearOfInsp == hdr.FshYearOfInsp && o.FcidAiAssetGrpCode == "CW" && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                  && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 3
                                  select 1).Count();
            var Length = (from o in _context.RmFormFcInsDtl
                          join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                          where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CW" && o.FcidActiveYn == true
                         && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true && h.FcihSubmitSts == true
                          select o.FcidLength)?.Sum();
            var AvgWidth = (from o in _context.RmFormFcInsDtl
                            join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                            where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CW" && o.FcidActiveYn == true
                         && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true && h.FcihSubmitSts == true
                            select o.FcidWidth)?.Average();
            //double Length = (double)(condition1 + condition2 + condition3);
            //var json = (from h in _context.RmFormFcInsHdr
            //            where h.FcihRoadCode == hdr.FshRoadCode
            //            && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true
            //            && h.FcihAssetTypes != null && h.FcihAssetTypes != "" && h.FcihSubmitSts == true
            //            select h.FcihAssetTypes).FirstOrDefault();
            //double avgWidth = 0;
            //if (!string.IsNullOrEmpty(json))
            //{
            //    var AvgWidth = Common.Utility.JDeSerialize<FormAssetTypesDTO>(json ?? "");

            //    if (AvgWidth.ContainsKey("CW"))
            //    {
            //        var cw = AvgWidth["CW"];
            //        foreach (var c in cw)
            //        {
            //            if (c.ContainsValue(grptype))
            //            {
            //                if (c.ContainsKey("AvgWidth"))
            //                {
            //                    double.TryParse(c["AvgWidth"], out avgWidth);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "CARRIAGE WAY",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "CW",
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "km",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }


        public RmFormFsInsDtl GetCenterLineMarkingDetail(string grptype, string StructureCode, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormFcInsDtl
                         join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                         where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CLM" && h.FcihActiveYn == true && h.FcihSubmitSts == true
                         && h.FcihYearOfInsp == hdr.FshYearOfInsp && o.FcidActiveYn == true
                         && h.FcihRoadCode == hdr.FshRoadCode
                         select 1).Count();
            decimal condition1 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CLM"
                                  && h.FcihYearOfInsp == hdr.FshYearOfInsp && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                  && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 1
                                  select 1).Count();
            decimal condition2 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CLM"
                                  && h.FcihYearOfInsp == hdr.FshYearOfInsp && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                  && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 2
                                  select 1).Count();
            decimal condition3 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CLM"
                                  && h.FcihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FcidActiveYn == true && h.FcihActiveYn == true
                                  && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 3 && h.FcihSubmitSts == true
                                  select 1).Count();
            var Length = (from o in _context.RmFormFcInsDtl
                          join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                          where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CLM" && o.FcidActiveYn == true
                         && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true && h.FcihSubmitSts == true
                          select o.FcidLength)?.Sum();
            var AvgWidth = (from o in _context.RmFormFcInsDtl
                            join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                            where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "CLM" && o.FcidActiveYn == true
                         && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true && h.FcihSubmitSts == true
                            select o.FcidWidth)?.Average();
            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "CENTER LINE MARKING",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "CLM",
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "km",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }

        public RmFormFsInsDtl GetEDGELineMarkingDetail(string grptype, string StructureCode, string LR, int userid, int headerid, RmFormFsInsHdr hdr)
        {

            var count = (from o in _context.RmFormFcInsDtl
                         join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                         where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "ELM" && o.FcidActiveYn == true
                         && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true && h.FcihSubmitSts == true
                         && o.FcidAiBound == LR
                         select 1).Count();
            decimal condition1 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "ELM" && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                  && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && o.FcidCondition == 1
                                  && o.FcidAiBound == LR
                                  select 1).Count();
            decimal condition2 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "ELM" && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                  && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && o.FcidCondition == 2
                                  && o.FcidAiBound == LR
                                  select 1).Count();
            decimal condition3 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && h.FcihYearOfInsp == hdr.FshYearOfInsp && o.FcidAiAssetGrpCode == "ELM" && o.FcidActiveYn == true && h.FcihSubmitSts == true && h.FcihActiveYn == true
                                  && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 3
                                  && o.FcidAiBound == LR
                                  select 1).Count();

            var Length = (from o in _context.RmFormFcInsDtl
                          join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                          where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "ELM" && o.FcidActiveYn == true && o.FcidAiBound == LR
                         && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true && h.FcihSubmitSts == true
                          select o.FcidLength)?.Sum();
            var AvgWidth = (from o in _context.RmFormFcInsDtl
                            join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                            where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == "ELM" && o.FcidActiveYn == true && o.FcidAiBound == LR
                         && h.FcihRoadCode == hdr.FshRoadCode && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true && h.FcihSubmitSts == true
                            select o.FcidWidth)?.Average();
            //double Length = (double)(condition1 + condition2 + condition3);
            //var json = (from h in _context.RmFormFcInsHdr
            //            where h.FcihRoadCode == hdr.FshRoadCode && h.FcihActiveYn == true && h.FcihSubmitSts == true
            //            && h.FcihAssetTypes != null && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihAssetTypes != ""
            //            select h.FcihAssetTypes).FirstOrDefault();
            //double avgWidth = 0;
            //if (!string.IsNullOrEmpty(json))
            //{
            //    var AvgWidth = Common.Utility.JDeSerialize<FormAssetTypesDTO>(json ?? "");

            //    if (AvgWidth.ContainsKey("ELM"))
            //    {
            //        var cw = AvgWidth["ELM"];
            //        foreach (var c in cw)
            //        {
            //            if (c.ContainsValue(grptype))
            //            {
            //                if (LR == "L" && c.ContainsKey("LAvgWidth"))
            //                {
            //                    double.TryParse(c["LAvgWidth"], out avgWidth);
            //                    break;
            //                }
            //                else if (LR == "R" && c.ContainsKey("RAvgWidth"))
            //                {
            //                    double.TryParse(c["RAvgWidth"], out avgWidth);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "EDGELINE MARKING",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "ELM_" + LR,
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "km",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
            {
                return null;
            }
        }

        public RmFormFsInsDtl GetDrainDetail(string grptype, string StructureCode, string LR, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormFdInsDtl
                         join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                         where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                         && h.FdihYearOfInsp == hdr.FshYearOfInsp
                         && o.FdidAiAssetGrpCode == "DR" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                         select 1).Count();
            var Length = (from o in _context.RmFormFdInsDtl
                          join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                          where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                          && h.FdihYearOfInsp == hdr.FshYearOfInsp
                          && o.FdidAiAssetGrpCode == "DR" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                          select o.FdidLength)?.Sum();
            var AvgWidth = (from o in _context.RmFormFdInsDtl
                            join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                            where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                          && h.FdihYearOfInsp == hdr.FshYearOfInsp
                          && o.FdidAiAssetGrpCode == "DR" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                            select o.FdidWidth)?.Average();
            decimal condition1 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FdidAiAssetGrpCode == "DR" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 1
                                  select 1).Count();
            decimal condition2 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FdidAiAssetGrpCode == "DR" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 2
                                  select 1).Count();
            decimal condition3 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FdidAiAssetGrpCode == "DR" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 3
                                  select 1).Count();

            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "DRAIN",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "DR_" + LR,
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "km",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
            {
                return null;
            }
        }

        public RmFormFsInsDtl GetDitchDetail(string grptype, string StructureCode, string LR, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormFdInsDtl
                         join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                         where h.FdihRoadCode == hdr.FshRoadCode
                         && h.FdihYearOfInsp == hdr.FshYearOfInsp && o.FdidAiGrpType == grptype && o.FdidAiAssetGrpCode == "DI"
                         && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                         select 1).Count();
            var Length = (from o in _context.RmFormFdInsDtl
                          join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                          where h.FdihRoadCode == hdr.FshRoadCode
                          && h.FdihYearOfInsp == hdr.FshYearOfInsp && o.FdidAiGrpType == grptype && o.FdidAiAssetGrpCode == "DI"
                          && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                          select o.FdidLength)?.Sum();
            var AvgWidth = (from o in _context.RmFormFdInsDtl
                            join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                            where h.FdihRoadCode == hdr.FshRoadCode
                          && h.FdihYearOfInsp == hdr.FshYearOfInsp && o.FdidAiGrpType == grptype && o.FdidAiAssetGrpCode == "DI"
                          && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                            select o.FdidWidth)?.Average();
            decimal condition1 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp && h.FdihSubmitSts == true
                                  && o.FdidAiAssetGrpCode == "DI" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 1
                                  select 1).Count();
            decimal condition2 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp && h.FdihSubmitSts == true
                                  && o.FdidAiAssetGrpCode == "DI" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 2
                                  select 1).Count();
            decimal condition3 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp && h.FdihSubmitSts == true
                                  && o.FdidAiAssetGrpCode == "DI" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 3
                                  select 1).Count();
            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "DITCH",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "DI_" + LR,
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "km",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }

        public RmFormFsInsDtl GetShoulderDetail(string grptype, string StructureCode, string LR, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormFdInsDtl
                         join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                         where h.FdihRoadCode == hdr.FshRoadCode
                         && h.FdihYearOfInsp == hdr.FshYearOfInsp && o.FdidAiGrpType == grptype && o.FdidAiAssetGrpCode == "SH"
                         && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                         select 1).Count();
            var Length = (from o in _context.RmFormFdInsDtl
                          join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                          where h.FdihRoadCode == hdr.FshRoadCode
                          && h.FdihYearOfInsp == hdr.FshYearOfInsp && o.FdidAiGrpType == grptype && o.FdidAiAssetGrpCode == "SH"
                          && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                          select o.FdidLength)?.Sum();
            var AvgWidth = (from o in _context.RmFormFdInsDtl
                            join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                            where h.FdihRoadCode == hdr.FshRoadCode
                          && h.FdihYearOfInsp == hdr.FshYearOfInsp && o.FdidAiGrpType == grptype && o.FdidAiAssetGrpCode == "SH"
                          && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                            select o.FdidWidth)?.Average();
            decimal condition1 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FdidAiAssetGrpCode == "SH" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 1
                                  select 1).Count();
            decimal condition2 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FdidAiAssetGrpCode == "SH" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 2
                                  select 1).Count();
            decimal condition3 = (from o in _context.RmFormFdInsDtl
                                  join h in _context.RmFormFdInsHdr on o.FdidFdihPkRefNo equals h.FdihPkRefNo
                                  where h.FdihRoadCode == hdr.FshRoadCode && o.FdidAiGrpType == grptype
                                  && h.FdihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FdidAiAssetGrpCode == "SH" && o.FdidAiBound == LR && o.FdidActiveYn == true && h.FdihActiveYn == true && h.FdihSubmitSts == true
                                  && o.FdidCondition == 3
                                  select 1).Count();
            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "SHOULDER",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "SH_" + LR,
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "km",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }

        public RmFormFsInsDtl GetRoadStudsDetail(string grptype, string StructureCode, string LR, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormFcInsDtl
                         join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                         where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == StructureCode && o.FcidActiveYn == true && h.FcihActiveYn == true
                          && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihRoadCode == hdr.FshRoadCode && h.FcihSubmitSts == true
                         select 1).Count();
            decimal condition1 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == StructureCode && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                   && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 1
                                  select o.FcidCondition).Count();
            decimal condition2 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == StructureCode && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                   && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 2
                                  select 1).Count();
            decimal condition3 = (from o in _context.RmFormFcInsDtl
                                  join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                                  where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == StructureCode && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                   && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihRoadCode == hdr.FshRoadCode && o.FcidCondition == 3
                                  select 1).Count();
            //string LR = grptype == "Left" ? "L" : grptype == "Right" ? "R" : "C";
            var Length = (from o in _context.RmFormFcInsDtl
                          join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                          where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == StructureCode && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                   && h.FcihYearOfInsp == hdr.FshYearOfInsp  && h.FcihRoadCode == hdr.FshRoadCode
                          select o.FcidLength)?.Sum();
            var AvgWidth = (from o in _context.RmFormFcInsDtl
                            join h in _context.RmFormFcInsHdr on o.FcidFcihPkRefNo equals h.FcihPkRefNo
                            where o.FcidAiGrpType == grptype && o.FcidAiAssetGrpCode == StructureCode && o.FcidActiveYn == true && h.FcihActiveYn == true && h.FcihSubmitSts == true
                                   && h.FcihYearOfInsp == hdr.FshYearOfInsp  && h.FcihRoadCode == hdr.FshRoadCode
                            select o.FcidWidth)?.Average();
            //double Length = (double)(condition1 + condition2 + condition3);

            //var json = (from h in _context.RmFormFcInsHdr
            //            where h.FcihRoadCode == hdr.FshRoadCode
            //             && h.FcihYearOfInsp == hdr.FshYearOfInsp && h.FcihActiveYn == true && h.FcihSubmitSts == true
            //            && h.FcihAssetTypes != null && h.FcihAssetTypes != ""
            //            select h.FcihAssetTypes).FirstOrDefault();
            //double avgWidth = 0;
            //if (!string.IsNullOrEmpty(json))
            //{
            //    var AvgWidth = Common.Utility.JDeSerialize<FormAssetTypesDTO>(json ?? "");

            //    if (AvgWidth.ContainsKey("RS"))
            //    {
            //        var cw = AvgWidth["RS"];
            //        foreach (var c in cw)
            //        {
            //            if (c.ContainsValue(grptype))
            //            {
            //                if (LR == "L" && c.ContainsKey("LAvgWidth"))
            //                {
            //                    double.TryParse(c["LAvgWidth"], out avgWidth);
            //                    break;
            //                }
            //                else if (LR == "R" && c.ContainsKey("RAvgWidth"))
            //                {
            //                    double.TryParse(c["RAvgWidth"], out avgWidth);
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "ROAD STUDS",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "R_" + LR,
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "km",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
            {
                return null;
            }
        }

        public RmFormFsInsDtl GetCulvertDetail(string grptype, string StructureCode, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormF4InsDtl
                         join h in _context.RmFormF4InsHdr on o.FivadFivahPkRefNo equals h.FivahPkRefNo
                         where
                         h.FivahRoadCode == hdr.FshRoadCode
                          && h.FivahYearOfInsp == hdr.FshYearOfInsp && o.FivadActiveYn == true && h.FivahActiveYn == true && o.FivadStrucCode == StructureCode && h.FivahSubmitSts == true
                         select 1).Count();
            var Length = (from o in _context.RmFormF4InsDtl
                          join h in _context.RmFormF4InsHdr on o.FivadFivahPkRefNo equals h.FivahPkRefNo
                          where
                          h.FivahRoadCode == hdr.FshRoadCode
                           && h.FivahYearOfInsp == hdr.FshYearOfInsp && o.FivadActiveYn == true && o.FivadStrucCode == StructureCode && h.FivahActiveYn == true && h.FivahSubmitSts == true
                          select o.FivadLength)?.Count();
            var AvgWidth = (from o in _context.RmFormF4InsDtl
                            join h in _context.RmFormF4InsHdr on o.FivadFivahPkRefNo equals h.FivahPkRefNo
                            where h.FivahRoadCode == hdr.FshRoadCode && h.FivahYearOfInsp == hdr.FshYearOfInsp && h.FivahSubmitSts == true && o.FivadActiveYn == true && h.FivahActiveYn == true && o.FivadStrucCode == StructureCode && h.FivahSubmitSts == true
                            select o.FivadWidth)?.Average();
            decimal condition1 =
                (from o in _context.RmFormF4InsDtl
                 join h in _context.RmFormF4InsHdr on o.FivadFivahPkRefNo equals h.FivahPkRefNo
                 where h.FivahRoadCode == hdr.FshRoadCode && h.FivahYearOfInsp == hdr.FshYearOfInsp && h.FivahSubmitSts == true && o.FivadActiveYn == true && h.FivahActiveYn == true && o.FivadStrucCode == StructureCode && h.FivahSubmitSts == true
                 && o.FivadCondition == 1
                 select 1).Count();
            decimal condition2 =
                (from o in _context.RmFormF4InsDtl
                 join h in _context.RmFormF4InsHdr on o.FivadFivahPkRefNo equals h.FivahPkRefNo
                 where h.FivahRoadCode == hdr.FshRoadCode && h.FivahYearOfInsp == hdr.FshYearOfInsp && h.FivahSubmitSts == true && o.FivadActiveYn == true && h.FivahActiveYn == true && o.FivadStrucCode == StructureCode && h.FivahSubmitSts == true
                 && o.FivadCondition == 2
                 select 1).Count();
            decimal condition3 =
                (from o in _context.RmFormF4InsDtl
                 join h in _context.RmFormF4InsHdr on o.FivadFivahPkRefNo equals h.FivahPkRefNo
                 where h.FivahRoadCode == hdr.FshRoadCode && h.FivahYearOfInsp == hdr.FshYearOfInsp && h.FivahSubmitSts == true && o.FivadActiveYn == true && h.FivahActiveYn == true && o.FivadStrucCode == StructureCode && h.FivahSubmitSts == true
                 && o.FivadCondition == 3
                 select 1).Count();
            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "CULVERTS",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "CV_" + StructureCode,
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "nr",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }

        public RmFormFsInsDtl GetBridgeDetail(string grptype, string StructureCode, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormF5InsDtl
                         join h in _context.RmFormF5InsHdr on o.FvadFvahPkRefNo equals h.FvahPkRefNo
                         where h.FvahRoadCode == hdr.FshRoadCode
                         && h.FvahYearOfInsp == hdr.FshYearOfInsp && o.FvadActiveYn == true && h.FvahActiveYn == true && o.FvadStrucCode == StructureCode && h.FvahSubmitSts == true
                         select 1).Count();
            var Length = (from o in _context.RmFormF5InsDtl
                          join h in _context.RmFormF5InsHdr on o.FvadFvahPkRefNo equals h.FvahPkRefNo
                          where h.FvahRoadCode == hdr.FshRoadCode
                          && h.FvahYearOfInsp == hdr.FshYearOfInsp && o.FvadActiveYn == true && h.FvahActiveYn == true && o.FvadStrucCode == StructureCode && h.FvahSubmitSts == true
                          select o.FvadLength)?.Count();
            var AvgWidth = (from o in _context.RmFormF5InsDtl
                            join h in _context.RmFormF5InsHdr on o.FvadFvahPkRefNo equals h.FvahPkRefNo
                            where h.FvahRoadCode == hdr.FshRoadCode
                            && h.FvahYearOfInsp == hdr.FshYearOfInsp && o.FvadActiveYn == true && h.FvahActiveYn == true && o.FvadStrucCode == StructureCode && h.FvahSubmitSts == true
                            select o.FvadWidth)?.Average();
            decimal condition1 =
                (from o in _context.RmFormF5InsDtl
                 join h in _context.RmFormF5InsHdr on o.FvadFvahPkRefNo equals h.FvahPkRefNo
                 where h.FvahRoadCode == hdr.FshRoadCode
                 && h.FvahYearOfInsp == hdr.FshYearOfInsp && o.FvadActiveYn == true && h.FvahActiveYn == true && o.FvadStrucCode == StructureCode && h.FvahSubmitSts == true
                 && o.FvadCondition == 1
                 select 1).Count();
            decimal condition2 =
               (from o in _context.RmFormF5InsDtl
                join h in _context.RmFormF5InsHdr on o.FvadFvahPkRefNo equals h.FvahPkRefNo
                where h.FvahRoadCode == hdr.FshRoadCode
                && h.FvahYearOfInsp == hdr.FshYearOfInsp && o.FvadActiveYn == true && h.FvahActiveYn == true && o.FvadStrucCode == StructureCode && h.FvahSubmitSts == true
                && o.FvadCondition == 2
                select 1).Count();
            decimal condition3 =
               (from o in _context.RmFormF5InsDtl
                join h in _context.RmFormF5InsHdr on o.FvadFvahPkRefNo equals h.FvahPkRefNo
                where h.FvahRoadCode == hdr.FshRoadCode
                && h.FvahYearOfInsp == hdr.FshYearOfInsp && o.FvadActiveYn == true && h.FvahActiveYn == true && o.FvadStrucCode == StructureCode && h.FvahSubmitSts == true
                && o.FvadCondition == 3
                select 1).Count();

            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = condition1,
                    FsdCondition2 = condition2,
                    FsdCondition3 = condition3,
                    FsdFeature = "BRIDGES",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = StructureCode,
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "nr",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }

        public RmFormFsInsDtl GetGuardialDetail(string grptype, string StructureCode, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormF2GrInsDtl
                         join h in _context.RmFormF2GrInsHdr on o.FgridFgrihPkRefNo equals h.FgrihPkRefNo
                         where h.FgrihRoadCode == hdr.FshRoadCode && h.FgrihYearOfInsp == hdr.FshYearOfInsp
                         && o.FgridActiveYn == true && o.FgridGrCode == StructureCode && h.FgrihActiveYn == true && h.FgrihSubmitSts == true
                         select 1).Count();
            var Length = (from o in _context.RmFormF2GrInsDtl
                          join h in _context.RmFormF2GrInsHdr on o.FgridFgrihPkRefNo equals h.FgrihPkRefNo
                          where h.FgrihRoadCode == hdr.FshRoadCode && h.FgrihYearOfInsp == hdr.FshYearOfInsp
                          && o.FgridActiveYn == true && o.FgridGrCode == StructureCode && h.FgrihActiveYn == true && h.FgrihSubmitSts == true
                          select o.FgridLength)?.Sum();
            double? AvgWidth = null;
            double? condition1 = (from o in _context.RmFormF2GrInsDtl
                                  join h in _context.RmFormF2GrInsHdr on o.FgridFgrihPkRefNo equals h.FgrihPkRefNo
                                  where h.FgrihRoadCode == hdr.FshRoadCode && h.FgrihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FgridActiveYn == true && o.FgridGrCode == StructureCode && h.FgrihActiveYn == true && h.FgrihSubmitSts == true
                                  select o.FgridGrCondition1)?.Sum();
            double? condition2 = (from o in _context.RmFormF2GrInsDtl
                                  join h in _context.RmFormF2GrInsHdr on o.FgridFgrihPkRefNo equals h.FgrihPkRefNo
                                  where h.FgrihRoadCode == hdr.FshRoadCode && h.FgrihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FgridActiveYn == true && o.FgridGrCode == StructureCode && h.FgrihActiveYn == true && h.FgrihSubmitSts == true
                                  select o.FgridGrCondition2)?.Sum();
            double? condition3 = (from o in _context.RmFormF2GrInsDtl
                                  join h in _context.RmFormF2GrInsHdr on o.FgridFgrihPkRefNo equals h.FgrihPkRefNo
                                  where h.FgrihRoadCode == hdr.FshRoadCode && h.FgrihYearOfInsp == hdr.FshYearOfInsp
                                  && o.FgridActiveYn == true && o.FgridGrCode == StructureCode && h.FgrihActiveYn == true && h.FgrihSubmitSts == true
                                  select o.FgridGrCondition3)?.Sum();
            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = (decimal?)condition1,
                    FsdCondition2 = (decimal?)condition2,
                    FsdCondition3 = (decimal?)condition3,
                    FsdFeature = "GUARDRAIL",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = StructureCode,
                    FsdLength = Length,
                    FsdWidth = AvgWidth,
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "m",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }

        //RW/S
        public RmFormFsInsDtl GetRetainingWall(string grptype, string StructureCode, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormF1Dtl
                         join h in _context.RmFormF1Hdr on o.Ff1dFf1hPkRefNo equals h.Ff1hPkRefNo
                         //join r in _context.RmFormR1Hdr on o.Ff1dR1hPkRefNo  equals r.Fr1hPkRefNo
                         where h.Ff1hRdCode == hdr.FshRoadCode && h.Ff1hInspectedYear  == hdr.FshYearOfInsp
                         && h.Ff1hActiveYn == true && o.Ff1dCode == StructureCode && h.Ff1hSubmitSts == true
                         select 1).Count();
       
            //var Length = (from o in _context.RmFormF1Dtl
            //              join h in _context.RmFormF1Hdr on o.Ff1dFf1hPkRefNo equals h.Ff1hPkRefNo
            //              join r in _context.RmFormR1Hdr on o.Ff1dR1hPkRefNo equals r.Fr1hPkRefNo
            //              join a in _context.RmAllassetInventory on o.Ff1dAssetId equals a.AiAssetId
            //              where h.Ff1hRdCode == hdr.FshRoadCode && h.Ff1hInspectedYear == hdr.FshYearOfInsp
            //             && h.Ff1hActiveYn == true && r.Fr1hAiStrucCode == StructureCode && h.Ff1hSubmitSts == true
            //              select a.AiLength)?.Sum();


            var AvgWidth = (from o in _context.RmFormF1Dtl
                            join h in _context.RmFormF1Hdr on o.Ff1dFf1hPkRefNo equals h.Ff1hPkRefNo
                            //join r in _context.RmFormR1Hdr on o.Ff1dR1hPkRefNo equals r.Fr1hPkRefNo
                            join a in _context.RmAllassetInventory on o.Ff1dAssetId equals a.AiAssetId
                            where h.Ff1hRdCode == hdr.FshRoadCode && h.Ff1hInspectedYear == hdr.FshYearOfInsp
                           && h.Ff1hActiveYn == true && o.Ff1dCode == StructureCode && h.Ff1hSubmitSts == true
                            select a.AiWidth)?.Sum();

            int? condition1 = (from o in _context.RmFormF1Dtl
                              join h in _context.RmFormF1Hdr on o.Ff1dFf1hPkRefNo equals h.Ff1hPkRefNo
                              //join r in _context.RmFormR1Hdr on o.Ff1dR1hPkRefNo equals r.Fr1hPkRefNo
                              join a in _context.RmAllassetInventory on o.Ff1dAssetId equals a.AiAssetId
                              where h.Ff1hRdCode == hdr.FshRoadCode && h.Ff1hInspectedYear == hdr.FshYearOfInsp
                             && h.Ff1hActiveYn == true && o.Ff1dCode == StructureCode && o.Ff1dOverallCondition == 1 && h.Ff1hSubmitSts == true
                               select 1)?.Count();

            int? condition2 = (from o in _context.RmFormF1Dtl
                               join h in _context.RmFormF1Hdr on o.Ff1dFf1hPkRefNo equals h.Ff1hPkRefNo
                               //join r in _context.RmFormR1Hdr on o.Ff1dR1hPkRefNo equals r.Fr1hPkRefNo
                               join a in _context.RmAllassetInventory on o.Ff1dAssetId equals a.AiAssetId
                               where h.Ff1hRdCode == hdr.FshRoadCode && h.Ff1hInspectedYear == hdr.FshYearOfInsp
                              && h.Ff1hActiveYn == true && o.Ff1dCode == StructureCode && o.Ff1dOverallCondition == 2 && h.Ff1hSubmitSts == true
                               select 1)?.Count();

            int? condition3 = (from o in _context.RmFormF1Dtl
                               join h in _context.RmFormF1Hdr on o.Ff1dFf1hPkRefNo equals h.Ff1hPkRefNo
                               //join r in _context.RmFormR1Hdr on o.Ff1dR1hPkRefNo equals r.Fr1hPkRefNo
                               join a in _context.RmAllassetInventory on o.Ff1dAssetId equals a.AiAssetId
                               where h.Ff1hRdCode == hdr.FshRoadCode && h.Ff1hInspectedYear == hdr.FshYearOfInsp
                              && h.Ff1hActiveYn == true && o.Ff1dCode == StructureCode && o.Ff1dOverallCondition == 3 && h.Ff1hSubmitSts == true
                               select 1)?.Count();

            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = (decimal?)condition1,
                    FsdCondition2 = (decimal?)condition2,
                    FsdCondition3 = (decimal?)condition3,
                    FsdFeature = "RETAINING WALL",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "rw",
                    FsdLength = count, //Length,
                    FsdWidth = (double?)Math.Round(((decimal)AvgWidth /count),2),
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "m",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }

        public RmFormFsInsDtl GetSigns(string grptype, string StructureCode, int userid, int headerid, RmFormFsInsHdr hdr)
        {
            var count = (from o in _context.RmFormF3Dtl
                         join h in _context.RmFormF3Hdr on o.Ff3dFf3hPkRefNo equals h.Ff3hPkRefNo
                         where h.Ff3hRdCode == hdr.FshRoadCode && h.Ff3hInspectedYear == hdr.FshYearOfInsp
                         && h.Ff3hActiveYn == true && o.Ff3dCode == StructureCode  && h.Ff3hSubmitSts == true
                         select 1).Count();

            var Length = count ;

            var AvgWidth = (from o in _context.RmFormF3Dtl
                            join h in _context.RmFormF3Hdr on o.Ff3dFf3hPkRefNo equals h.Ff3hPkRefNo
                            join i in _context.RmAllassetInventory on o.Ff3dAssetId equals i.AiPkRefNo.ToString()
                            where h.Ff3hRdCode == hdr.FshRoadCode && h.Ff3hInspectedYear == hdr.FshYearOfInsp && i.AiActiveYn == true &&  o.Ff3dCode == StructureCode && h.Ff3hSubmitSts == true
                            select i.AiWidth)?.Sum();


            int? condition1 = (from o in _context.RmFormF3Dtl
                               join h in _context.RmFormF3Hdr on o.Ff3dFf3hPkRefNo equals h.Ff3hPkRefNo
                               where h.Ff3hRdCode == hdr.FshRoadCode && h.Ff3hInspectedYear == hdr.FshYearOfInsp
                              && h.Ff3hActiveYn == true && o.Ff3dCode == StructureCode && o.Ff3dConditionI == 1 && h.Ff3hSubmitSts == true
                               select 1)?.Count();

            int? condition2 = (from o in _context.RmFormF3Dtl
                               join h in _context.RmFormF3Hdr on o.Ff3dFf3hPkRefNo equals h.Ff3hPkRefNo
                               where h.Ff3hRdCode == hdr.FshRoadCode && h.Ff3hInspectedYear == hdr.FshYearOfInsp
                              && h.Ff3hActiveYn == true && o.Ff3dCode == StructureCode && o.Ff3dConditionI == 2 && h.Ff3hSubmitSts == true
                               select 1)?.Count();

            int? condition3 = (from o in _context.RmFormF3Dtl
                                join h in _context.RmFormF3Hdr on o.Ff3dFf3hPkRefNo equals h.Ff3hPkRefNo
                                where h.Ff3hRdCode == hdr.FshRoadCode && h.Ff3hInspectedYear == hdr.FshYearOfInsp
                               && h.Ff3hActiveYn == true && o.Ff3dCode == StructureCode && o.Ff3dConditionI == 3 && h.Ff3hSubmitSts == true
                               select 1)?.Count();


            if (count > 0)
            {
                var detail = new RmFormFsInsDtl
                {
                    FsdActiveYn = true,
                    FsdCondition1 = (decimal?)condition1,
                    FsdCondition2 = (decimal?)condition2,
                    FsdCondition3 = (decimal?)condition3,
                    FsdFeature = "SIGNS",
                    FsdFshPkRefNo = headerid,
                    FsdGrpType = grptype,
                    FsdGrpCode = "sg",
                    FsdLength = Length,
                    FsdWidth = (double?)Math.Round(((decimal)AvgWidth /count),2),
                    FsdStrucCode = StructureCode,
                    FsdSubmitSts = false,
                    FsdUnit = "nr",
                    FsdCrBy = userid,
                    FsdCrDt = DateTime.UtcNow,
                    FsdModBy = userid,
                    FsdModDt = DateTime.UtcNow
                };
                return detail;
            }
            else
                return null;
        }
    }
}
