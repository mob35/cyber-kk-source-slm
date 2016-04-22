using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLM.Resource;

namespace SLM.Dal.Models
{
    public class KKSlmPhoneCallModel
    {
        private SLM_DBEntities slmdb = null;

        public KKSlmPhoneCallModel()
        {
            slmdb = new SLM_DBEntities();
        }

        public void InsertPhoneCallHistory(string ticketId, string cardType, string cardId, string leadStatusCode, string oldstatus, string ownerBranch, string owner, string oldOwner, string delegateLeadBranch, string delegateLead, string oldDelegateLead, string contactPhone, string contactDetail, string createBy)
        {
            try
            {
                kkslm_tr_lead lead = null;
                DateTime createDate = DateTime.Now;
                int? createdByPositionId = null;
                int? ownerPositionId = null;

                var cusinfo = slmdb.kkslm_tr_cusinfo.Where(p => p.slm_TicketId == ticketId).FirstOrDefault();
                if (cusinfo != null)
                {
                    if (cardType != "")
                        cusinfo.slm_CardType = int.Parse(cardType);
                    else
                        cusinfo.slm_CardType = null;

                    string oldCitizenId = string.IsNullOrEmpty(cusinfo.slm_CitizenId) ? null : cusinfo.slm_CitizenId;
                    cusinfo.slm_CitizenId = cardId != "" ? cardId : null;

                    if (oldCitizenId != cusinfo.slm_CitizenId)
                        KKSlmTrHistoryModel.InsertHistory(slmdb, ticketId, SLMConstant.HistoryTypeCode.UpdateCardId, oldCitizenId, cusinfo.slm_CitizenId, createBy, createDate);
                }

                kkslm_phone_call phone = new kkslm_phone_call();
                phone.slm_TicketId = ticketId;
                phone.slm_ContactPhone = contactPhone;
                phone.slm_ContactDetail = contactDetail;
                phone.slm_Status = leadStatusCode;
                phone.slm_Owner = owner;
                ownerPositionId = GetPositionId(owner);
                phone.slm_Owner_Position = ownerPositionId;
                phone.slm_CreateDate = createDate;
                phone.slm_CreateBy = createBy;
                createdByPositionId = GetPositionId(createBy);
                phone.slm_CreatedBy_Position = createdByPositionId;
                phone.is_Deleted = 0;
                slmdb.kkslm_phone_call.AddObject(phone);

                if (lead == null)
                    lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();

                if (leadStatusCode != oldstatus)
                {
                    kkslm_tr_activity activity = new kkslm_tr_activity();
                    activity.slm_TicketId = ticketId;
                    activity.slm_OldStatus = oldstatus;
                    activity.slm_NewStatus = leadStatusCode;
                    activity.slm_CreatedBy = createBy;
                    activity.slm_CreatedBy_Position = createdByPositionId;
                    activity.slm_CreatedDate = createDate;
                    activity.slm_Type = SLMConstant.ActionType.ChangeStatus;                //02
                    activity.slm_SystemAction = SLMConstant.SystemName;        //System ที่เข้ามาทำ action (19/03/2015)
                    activity.slm_SystemActionBy = SLMConstant.SystemName;      //action เกิดขึ้นที่ระบบอะไร (19/03/2015)

                    activity.slm_ExternalSystem_Old = lead.slm_ExternalSystem;                //add 14/10/2015
                    activity.slm_ExternalStatus_Old = lead.slm_ExternalStatus;                //add 14/10/2015
                    activity.slm_ExternalSubStatus_Old = lead.slm_ExternalSubStatus;          //add 14/10/2015
                    activity.slm_ExternalSubStatusDesc_Old = lead.slm_ExternalSubStatusDesc;  //add 14/10/2015

                    lead.slm_ExternalSystem = null;             //add 14/10/2015
                    lead.slm_ExternalStatus = null;             //add 14/10/2015
                    lead.slm_ExternalSubStatus = null;          //add 14/10/2015
                    lead.slm_ExternalSubStatusDesc = null;      //add 14/10/2015

                    slmdb.kkslm_tr_activity.AddObject(activity);

                    if (lead == null)
                        lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();

                    if (lead != null)
                    {
                        lead.slm_Status = leadStatusCode;
                        lead.slm_StatusBy = createBy;
                        lead.slm_StatusDate = createDate;
                        lead.slm_Counting = 0;
                        lead.slm_UpdatedBy = createBy;
                        lead.slm_UpdatedDate = createDate;
                    }

                    KKSlmTrHistoryModel.InsertHistory(slmdb, ticketId, SLMConstant.HistoryTypeCode.UpdateStatus, oldstatus, leadStatusCode, createBy, createDate);
                }

                if (owner != oldOwner)
                {
                    kkslm_tr_activity activity = new kkslm_tr_activity();
                    activity.slm_TicketId = ticketId;
                    if (!string.IsNullOrEmpty(oldOwner))
                    {
                        activity.slm_OldOwner = oldOwner;
                        activity.slm_OldOwner_Position = GetPositionId(oldOwner);
                    }
                    activity.slm_NewOwner = owner;
                    activity.slm_NewOwner_Position = ownerPositionId;
                    activity.slm_CreatedBy = createBy;
                    activity.slm_CreatedBy_Position = createdByPositionId;
                    activity.slm_CreatedDate = createDate;
                    activity.slm_Type = SLMConstant.ActionType.ChangeOwner;
                    activity.slm_SystemAction = SLMConstant.SystemName;        //System ที่เข้ามาทำ action (19/03/2015)
                    activity.slm_SystemActionBy = SLMConstant.SystemName;      //action เกิดขึ้นที่ระบบอะไร (19/03/2015)
                    slmdb.kkslm_tr_activity.AddObject(activity);

                    if (lead == null)
                        lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();

                    if (lead != null)
                    {
                        lead.slm_StaffId = slmdb.kkslm_ms_staff.Where(p => p.slm_UserName == owner).Select(p => p.slm_StaffId).FirstOrDefault();
                        lead.slm_Owner = owner;
                        lead.slm_Owner_Branch = ownerBranch;
                        lead.slm_Owner_Position = ownerPositionId;
                        lead.slm_AssignedFlag = "0";
                        lead.slm_AssignedDate = null;
                        lead.slm_AssignedBy = null;
                        lead.slm_OldOwner = oldOwner;
                    }

                    KKSlmTrHistoryModel.InsertHistory(slmdb, ticketId, SLMConstant.HistoryTypeCode.UpdateOwner, oldOwner, owner, createBy, createDate);
                }

                if (delegateLead != oldDelegateLead)
                {
                    kkslm_tr_activity activity = new kkslm_tr_activity();
                    activity.slm_TicketId = ticketId;
                    if (!string.IsNullOrEmpty(oldDelegateLead))
                    {
                        activity.slm_OldDelegate = oldDelegateLead;
                        activity.slm_OldDelegate_Position = GetPositionId(oldDelegateLead);
                    }
                    activity.slm_NewDelegate = delegateLead;
                    activity.slm_NewDelegate_Position = GetPositionId(delegateLead);
                    activity.slm_CreatedBy = createBy;
                    activity.slm_CreatedBy_Position = createdByPositionId;
                    activity.slm_CreatedDate = createDate;
                    activity.slm_Type = SLMConstant.ActionType.Delegate;
                    activity.slm_SystemAction = SLMConstant.SystemName;        //System ที่เข้ามาทำ action (19/03/2015)
                    activity.slm_SystemActionBy = SLMConstant.SystemName;      //action เกิดขึ้นที่ระบบอะไร (19/03/2015)
                    slmdb.kkslm_tr_activity.AddObject(activity);

                    if (lead == null)
                        lead = slmdb.kkslm_tr_lead.Where(p => p.slm_ticketId == ticketId).FirstOrDefault();

                    if (lead != null)
                    {
                        lead.slm_Delegate_Flag = string.IsNullOrEmpty(delegateLead) ? 0 : 1;
                        if (!string.IsNullOrEmpty(delegateLead))
                            lead.slm_DelegateDate = createDate;
                        else
                            lead.slm_DelegateDate = null;

                        lead.slm_Delegate = string.IsNullOrEmpty(delegateLead) ? null : delegateLead;
                        lead.slm_Delegate_Branch = string.IsNullOrEmpty(delegateLead) ? null : delegateLeadBranch;
                        lead.slm_Delegate_Position = string.IsNullOrEmpty(delegateLead) ? null : GetPositionId(delegateLead);
                    }

                    KKSlmTrHistoryModel.InsertHistory(slmdb, ticketId, SLMConstant.HistoryTypeCode.UpdateDelegate, oldDelegateLead, delegateLead, createBy, createDate);
                }

                slmdb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int? GetPositionId(string username)
        {
            KKSlmMsStaffModel staff = new KKSlmMsStaffModel(slmdb);
            return staff.GetPositionId(username);
        }
    }
}
