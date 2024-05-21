using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Gym_Management_System.Models
{
    public class SaveData
    {
        public void insertData(CombineView combineView)
        {
            int rem = 0;
            var memberdata = new MemberTB
            {
                Name = combineView.member.Name,
                Phone=combineView.member.Phone,
                Email=combineView.member.Email,
                JoiningDate=combineView.member.JoiningDate,
                DOB=combineView.member.DOB,
                Address=combineView.member.Address,
                Gender=combineView.member.Gender
            };
            using(var db=new GYMDBEntities4())
            {
                db.MemberTBs.Add(memberdata);
                db.SaveChanges();
            }
            rem = combineView.transaction.OfferPrice - combineView.transaction.PaidAmount;
            GYMDBEntities4 db1 = new GYMDBEntities4();
            int id = db1.MemberTBs.OrderByDescending(e => e.ID).Select(e => e.ID).FirstOrDefault();
            var transacation = new TransactionTB
            {
                Membership = combineView.transaction.Membership,
                OfferPrice = combineView.transaction.OfferPrice,
                PaidAmount = combineView.transaction.PaidAmount,
                Days = combineView.transaction.Days,
                Trainer=combineView.transaction.Trainer,
                PaymentDate=combineView.transaction.PaymentDate,
                DueDate=combineView.transaction.DueDate,
                DueAmount=rem,
                PaymentMode=combineView.transaction.PaymentMode,
                M_ID=id
            };
            using(var db=new GYMDBEntities4())
            {
                db.TransactionTBs.Add(transacation);
                db.SaveChanges();
            }
        }
        public void UpdateData(CombineView combineView)
    {
        int rem = 0;
        
        // Check if the CombineView contains IDs for member and transaction
        if (combineView.member.ID != 0 && combineView.transaction.ID != 0)
        {
            using (var db = new GYMDBEntities4())
            {
                // Retrieve the existing member record from the database
                var existingMember = db.MemberTBs.FirstOrDefault(m => m.ID == combineView.member.ID);
                
                if (existingMember != null)
                {
                    // Update member properties
                    existingMember.Name = combineView.member.Name;
                    existingMember.Phone = combineView.member.Phone;
                    existingMember.Email = combineView.member.Email;
                    existingMember.JoiningDate = combineView.member.JoiningDate;
                    existingMember.DOB = combineView.member.DOB;
                    existingMember.Address = combineView.member.Address;
                    existingMember.Gender = combineView.member.Gender;
                }

                // Retrieve the existing transaction record from the database
                var existingTransaction = db.TransactionTBs.FirstOrDefault(t => t.ID == combineView.transaction.ID);
                
                if (existingTransaction != null)
                {
                    // Calculate remaining amount
                    rem = combineView.transaction.OfferPrice - combineView.transaction.PaidAmount;
                    
                    // Update transaction properties
                    existingTransaction.Membership = combineView.transaction.Membership;
                    existingTransaction.OfferPrice = combineView.transaction.OfferPrice;
                    existingTransaction.PaidAmount = combineView.transaction.PaidAmount;
                    existingTransaction.Days = combineView.transaction.Days;
                    existingTransaction.Trainer = combineView.transaction.Trainer;
                    existingTransaction.PaymentDate = combineView.transaction.PaymentDate;
                    existingTransaction.DueDate = combineView.transaction.DueDate;
                    existingTransaction.DueAmount = rem;
                    existingTransaction.PaymentMode = combineView.transaction.PaymentMode;
                }

                // Save changes to the database
                db.SaveChanges();
            }
        }

    }
    public void convertToMember(CombineView combineView)
    {
            int rem = 0;
            var memberdata = new MemberTB
            {
                Name = combineView.member.Name,
                Phone = combineView.member.Phone,
                Email = combineView.member.Email,
                JoiningDate = combineView.member.JoiningDate,
                DOB = combineView.member.DOB,
                Address = combineView.member.Address,
                Gender = combineView.member.Gender
            };
            using (var db = new GYMDBEntities4())
            {
                db.MemberTBs.Add(memberdata);
                db.SaveChanges();
            }
            rem = combineView.transaction.OfferPrice - combineView.transaction.PaidAmount;
            GYMDBEntities4 db1 = new GYMDBEntities4();
            int id = db1.MemberTBs.OrderByDescending(e => e.ID).Select(e => e.ID).FirstOrDefault();
            var transacation = new TransactionTB
            {
                Membership = combineView.transaction.Membership,
                OfferPrice = combineView.transaction.OfferPrice,
                PaidAmount = combineView.transaction.PaidAmount,
                Days = combineView.transaction.Days,
                Trainer = combineView.transaction.Trainer,
                PaymentDate = combineView.transaction.PaymentDate,
                DueDate = combineView.transaction.DueDate,
                DueAmount = rem,
                PaymentMode = combineView.transaction.PaymentMode,
                M_ID = id
            };
            using (var db = new GYMDBEntities4())
            {
                db.TransactionTBs.Add(transacation);
                db.SaveChanges();
            }
            using(var db=new GYMDBEntities4())
            {
                VisitorTB visitor = db.VisitorTBs.Find(combineView.member.ID);
                db.VisitorTBs.Remove(visitor);
                db.SaveChanges();
            }
        }
    }
}