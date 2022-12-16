﻿using RAMMS.Domain.Models;
using RAMMS.DTO.JQueryModel;
using RAMMS.Repository.Interfaces;
using RAMS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using RAMMS.Common;
using Microsoft.EntityFrameworkCore;
using RAMMS.Common.RefNumber;
using RAMMS.DTO.Report;
using RAMMS.DTO.ResponseBO;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAMMS.DTO.RequestBO;

namespace RAMMS.Repository
{
    public class FormB11Repository : RepositoryBase<RmB11Hdr>, IFormB11Repository
    {
        public FormB11Repository(RAMMSContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GridWrapper<object>> GetHeaderGrid(DataTableAjaxPostModel searchData)
        {

            int? MiriMaxID = (from rn in _context.RmB11Hdr where rn.B11hRmuCode == "Miri" select rn.B11hPkRefNo).DefaultIfEmpty().Max();
            int? BatuNiahMaxID = (from rn in _context.RmB11Hdr where rn.B11hRmuCode == "Batu Niah" select rn.B11hPkRefNo).DefaultIfEmpty().Max();
            var query = (from hdr in _context.RmB11Hdr

                         select new
                         {
                             RefNo = hdr.B11hPkRefNo,
                             RMUCode = hdr.B11hRmuCode,
                             RMUName = hdr.B11hRmuName,
                             RevisionYear = hdr.B11hRevisionYear,
                             RevisionNo = hdr.B11hRevisionNo,
                             RevisionDate = hdr.B11hRevisionDate,
                             CrByName = hdr.B11hCrByName,
                             MaxRecord = (hdr.B11hPkRefNo == MiriMaxID) || (hdr.B11hPkRefNo== BatuNiahMaxID)
                         });

            if (searchData.filter != null)
            {
                foreach (var item in searchData.filter.Where(x => !string.IsNullOrEmpty(x.Value)))
                {
                    string strVal = Utility.ToString(item.Value).Trim();
                    if(item.Key== "RMUName" && strVal =="MRI")
                    {
                        strVal = "Miri";
                    }
                    else if (item.Key == "RMUName" && strVal == "BTN")
                    {
                        strVal = "Batu Niah";
                    }
                    switch (item.Key)
                    {
                        case "KeySearch":
                            DateTime? dtSearch = Utility.ToDateTime(strVal);
                            query = query.Where(x =>
                                 (x.RevisionYear.HasValue ? x.RevisionYear.Value.ToString() : "").Contains(strVal)
                                 || (x.RMUName ?? "").Contains(strVal)
                                 || (x.RevisionNo.HasValue ? x.RevisionNo.Value.ToString() : "").Contains(strVal)
                                 || (x.RevisionDate.HasValue && ((x.RevisionDate.Value.ToString().Contains(strVal)) || (dtSearch.HasValue && x.RevisionDate == dtSearch)))
                                 || (x.CrByName ?? "").Contains(strVal)
                                 );
                            break;
                        case "fromRevDate":
                            DateTime? dtFrom = Utility.ToDateTime(strVal);
                            string toDate = Utility.ToString(searchData.filter["toRevDate"]);
                            if (toDate == "")
                                query = query.Where(x => x.RevisionDate >= dtFrom);
                            else
                            {
                                DateTime? dtTo = Utility.ToDateTime(toDate);
                                query = query.Where(x => x.RevisionDate >= dtFrom && x.RevisionDate <= dtTo);
                            }
                            break;
                        case "toRevDate":
                            string frmDate = Utility.ToString(searchData.filter["fromRevDate"]);
                            if (frmDate == "")
                            {
                                DateTime? dtTo = Utility.ToDateTime(strVal);
                                query = query.Where(x => x.RevisionDate <= dtTo);
                            }
                            break;

                        case "Year":
                            int iFYr = Utility.ToInt(strVal);
                            query = query.Where(x => x.RevisionYear.HasValue && x.RevisionYear == iFYr);
                            break;
                        default:
                            query = query.WhereEquals(item.Key, strVal);
                            break;
                    }
                }

            }
            GridWrapper<object> grid = new GridWrapper<object>();
            grid.recordsTotal = await query.CountAsync();
            grid.recordsFiltered = grid.recordsTotal;
            grid.draw = searchData.draw;
            grid.data = await query.Order(searchData, query.OrderByDescending(s => s.RefNo)).Skip(searchData.start)
                                .Take(searchData.length)
                                .ToListAsync(); ;

            return grid;
        }

        public RmB11Hdr GetHeaderById(int id, int IsEdit)
        {
            RmB11Hdr res = (from r in _context.RmB11Hdr where r.B11hPkRefNo == id select r).FirstOrDefault();
            int? RevNo = (from rn in _context.RmB11Hdr where rn.B11hRevisionYear == res.B11hRevisionYear && rn.B11hRmuCode == res.B11hRmuCode select rn.B11hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (IsEdit == 1)
                res.B11hRevisionNo = RevNo;
            res.RmB11CrewDayCostHeader = (from r in _context.RmB11CrewDayCostHeader where r.B11cdchB11hPkRefNo == id select r).ToList();
            res.RmB11LabourCost = (from r in _context.RmB11LabourCost where r.B11lcB11hPkRefNo == id select r).ToList();
            res.RmB11EquipmentCost = (from r in _context.RmB11EquipmentCost where r.B11ecB11hPkRefNo == id select r).ToList();
            res.RmB11MaterialCost = (from r in _context.RmB11MaterialCost where r.B11mcB11hPkRefNo == id select r).ToList();

            return res;
        }

        public int? GetMaxRev(int Year)
        {
            int? rev = (from rn in _context.RmB11Hdr where rn.B11hRevisionYear == Year select rn.B11hRevisionNo).DefaultIfEmpty().Max() + 1;
            if (rev == null)
                rev = 1;
            return rev;
        }

        public List<RmB7LabourHistory> GetLabourHistoryData(int year)
        {
            //int? RefNo = (from rn in _context.RmB7Hdr where rn.B7hRevisionYear == year select rn.B7hPkRefNo).DefaultIfEmpty().Max();
            int? RefNo = (from rn in _context.RmB7Hdr select rn.B7hPkRefNo).DefaultIfEmpty().Max();
            List<RmB7LabourHistory> res = (from r in _context.RmB7LabourHistory where r.B7lhB7hPkRefNo == RefNo select r).OrderBy(x => x.B7lhCode).ToList();
            return res;
        }
        public List<RmB7MaterialHistory> GetMaterialHistoryData(int year)
        {
            //int? RefNo = (from rn in _context.RmB7Hdr where rn.B7hRevisionYear == year select rn.B7hPkRefNo).DefaultIfEmpty().Max();
            int? RefNo = (from rn in _context.RmB7Hdr select rn.B7hPkRefNo).DefaultIfEmpty().Max();
            List<RmB7MaterialHistory> res = (from r in _context.RmB7MaterialHistory where r.B7mhB7hPkRefNo == RefNo select r).OrderBy(x => x.B7mhCode).ToList();
            return res;
        }

        public List<RmB7EquipmentsHistory> GetEquipmentHistoryData(int year)
        {
            //int? RefNo = (from rn in _context.RmB7Hdr where rn.B7hRevisionYear == year select rn.B7hPkRefNo).DefaultIfEmpty().Max();
            int? RefNo = (from rn in _context.RmB7Hdr select rn.B7hPkRefNo).DefaultIfEmpty().Max();
            List<RmB7EquipmentsHistory> res = (from r in _context.RmB7EquipmentsHistory where r.B7ehB7hPkRefNo == RefNo select r).OrderBy(x => x.B7ehCode).ToList();
            return res;
        }

        public List<RmB11LabourCost> GetLabourViewHistoryData(int id)
        {
            List<RmB11LabourCost> res = (from r in _context.RmB11LabourCost where r.B11lcB11hPkRefNo == id select r).OrderBy(x => x.B11lcLabourOrderId).OrderBy(x=>x.B11lcLabourId).ToList();
            return res;
        }
        public List<RmB11MaterialCost> GetMaterialViewHistoryData(int id)
        {
            List<RmB11MaterialCost> res = (from r in _context.RmB11MaterialCost where r.B11mcB11hPkRefNo == id select r).OrderBy(x => x.B11mcMaterialOrderId).OrderBy(x=>x.B11mcMaterialId).ToList();
            return res;
        }
        public List<RmB11EquipmentCost> GetEquipmentViewHistoryData(int id)
        {
            List<RmB11EquipmentCost> res = (from r in _context.RmB11EquipmentCost where r.B11ecB11hPkRefNo == id select r).OrderBy(x => x.B11ecEquipmentOrderId).OrderBy(x=>x.B11ecEquipmentId).ToList();
            return res;
        }
        public async Task<int> SaveFormB11(RmB11Hdr FormB11)
        {
            try
            {


                _context.RmB11Hdr.Add(FormB11);
                _context.SaveChanges();

                //foreach (var item in FormB7.RmB7LabourHistory.ToList())
                //{
                //    item.B7lhB7hPkRefNo = FormB7.B7hPkRefNo;
                //    _context.RmB7LabourHistory.Add(item);
                //    _context.SaveChanges();
                //}

                //foreach (var item in FormB7.RmB7MaterialHistory.ToList())
                //{
                //    item.B7mhB7hPkRefNo = FormB7.B7hPkRefNo;
                //    _context.RmB7MaterialHistory.Add(item);
                //    _context.SaveChanges();
                //}

                //foreach (var item in FormB7.RmB7EquipmentsHistory.ToList())
                //{
                //    item.B7ehB7hPkRefNo = FormB7.B7hPkRefNo;
                //    _context.RmB7EquipmentsHistory.Add(item);
                //    _context.SaveChanges();
                //}

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}