using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractMontlhyClaimStatus.Models;

namespace ContractMontlhyClaimStatus.Repositories
    {
        public static class ClaimRepository
        {
            private static readonly List<Claim> _claims = new();
            private static int _nextId = 1;

            public static IReadOnlyList<Claim> GetAll() => _claims.AsReadOnly();

            public static Claim GetById(int id) => _claims.FirstOrDefault(c => c.Id == id);

            public static Claim Add(Claim claim)
            {
                claim.Id = _nextId++;
                _claims.Add(claim);
                return claim;
            }

            public static void Approve(int id)
            {
                var claim = GetById(id);
                if (claim != null) claim.Status = "Approved";
            }

            public static void Reject(int id)
            {
                var claim = GetById(id);
                if (claim != null) claim.Status = "Rejected";
            }

            public static void ClearAll()
            {
                _claims.Clear();
                _nextId = 1;
            }
        }
    }
