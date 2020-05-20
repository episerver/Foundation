﻿using EPiServer.Logging;
using Mediachase.Data.Provider;
using Powells.CouponCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Foundation.Commerce.Marketing
{
    public class UniqueCouponService
    {
        private readonly IConnectionStringHandler _connectionHandler;
        private readonly ILogger _logger = LogManager.GetLogger(typeof(UniqueCouponService));
        private readonly CouponCodeBuilder _couponCodeBuilder = new CouponCodeBuilder();

        private const string IdColumn = "Id";
        private const string PromotionIdColumn = "PromotionId";
        private const string CodeColumn = "Code";
        private const string ValidColumn = "Valid";
        private const string ExpirationColumn = "Expiration";
        private const string CustomerIdColumn = "CustomerId";
        private const string CreatedColumn = "Created";
        private const string MaxRedemptionsColumn = "MaxRedemptions";
        private const string UsedRedemptionsColumn = "UsedRedemptions";

        public UniqueCouponService(IConnectionStringHandler connectionHandler) => _connectionHandler = connectionHandler;

        public bool SaveCoupons(List<UniqueCoupon> coupons)
        {
            try
            {
                var connectionString = _connectionHandler.Commerce.ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var command = new SqlCommand
                        {
                            Connection = transaction.Connection,
                            Transaction = transaction,
                            CommandType = CommandType.StoredProcedure,
                            CommandText = "UniqueCoupons_Save"
                        };
                        command.Parameters.Add(new SqlParameter("@Data", CreateUniqueCouponsDataTable(coupons)));
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception exn)
            {
                _logger.Error(exn.Message, exn);
            }

            return false;
        }

        public bool DeleteById(long id)
        {
            try
            {
                var connectionString = _connectionHandler.Commerce.ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var command = new SqlCommand
                        {
                            Connection = transaction.Connection,
                            Transaction = transaction,
                            CommandType = CommandType.StoredProcedure,
                            CommandText = "UniqueCoupons_DeleteById"
                        };
                        command.Parameters.Add(new SqlParameter("@Id", id));
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception exn)
            {
                _logger.Error(exn.Message, exn);
            }

            return false;
        }

        public bool DeleteByPromotionId(int id)
        {
            try
            {
                var connectionString = _connectionHandler.Commerce.ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var command = new SqlCommand
                        {
                            Connection = transaction.Connection,
                            Transaction = transaction,
                            CommandType = CommandType.StoredProcedure,
                            CommandText = "UniqueCoupons_DeleteByPromotionId"
                        };
                        command.Parameters.Add(new SqlParameter("@PromotionId", id));
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception exn)
            {
                _logger.Error(exn.Message, exn);
            }

            return false;
        }

        public List<UniqueCoupon> GetByPromotionId(int id)
        {
            try
            {
                var coupons = new List<UniqueCoupon>();
                var connectionString = _connectionHandler.Commerce.ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "UniqueCoupons_GetByPromotionId"
                    };
                    command.Parameters.Add(new SqlParameter("@PromotionId", id));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            coupons.Add(GetUniqueCoupon(reader));
                        }
                    }
                }

                return coupons;
            }
            catch (Exception exn)
            {
                _logger.Error(exn.Message, exn);
            }

            return null;
        }

        public UniqueCoupon GetById(long id)
        {
            try
            {
                UniqueCoupon coupon = null;
                var connectionString = _connectionHandler.Commerce.ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "UniqueCoupons_GetById"
                    };
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            coupon = GetUniqueCoupon(reader);
                        }
                    }
                }

                return coupon;
            }
            catch (Exception exn)
            {
                _logger.Error(exn.Message, exn);
            }

            return null;
        }

        public string GenerateCoupon()
        {
            return _couponCodeBuilder.Generate(new Options
            {
                Plaintext = "Foundation"
            });
        }

        private DataTable CreateUniqueCouponsDataTable(IEnumerable<UniqueCoupon> coupons)
        {
            var tblUniqueCoupon = new DataTable();
            tblUniqueCoupon.Columns.Add(new DataColumn(IdColumn, typeof(long)));
            tblUniqueCoupon.Columns.Add(PromotionIdColumn, typeof(int));
            tblUniqueCoupon.Columns.Add(CodeColumn, typeof(string));
            tblUniqueCoupon.Columns.Add(ValidColumn, typeof(DateTime));
            tblUniqueCoupon.Columns.Add(ExpirationColumn, typeof(DateTime));
            tblUniqueCoupon.Columns.Add(CustomerIdColumn, typeof(Guid));
            tblUniqueCoupon.Columns.Add(CreatedColumn, typeof(DateTime));
            tblUniqueCoupon.Columns.Add(MaxRedemptionsColumn, typeof(int));
            tblUniqueCoupon.Columns.Add(UsedRedemptionsColumn, typeof(int));

            foreach (var coupon in coupons)
            {
                var row = tblUniqueCoupon.NewRow();
                row[IdColumn] = coupon.Id;
                row[PromotionIdColumn] = coupon.PromotionId;
                row[CodeColumn] = coupon.Code;
                row[ValidColumn] = coupon.ValidFrom;
                row[ExpirationColumn] = coupon.Expiration ?? (object)DBNull.Value;
                row[CustomerIdColumn] = coupon.CustomerId ?? (object)DBNull.Value;
                row[CreatedColumn] = coupon.Created;
                row[MaxRedemptionsColumn] = coupon.MaxRedemptions;
                row[UsedRedemptionsColumn] = coupon.UsedRedemptions;
                tblUniqueCoupon.Rows.Add(row);
            }

            return tblUniqueCoupon;
        }

        private UniqueCoupon GetUniqueCoupon(IDataReader row)
        {
            return new UniqueCoupon
            {
                Code = row[CodeColumn].ToString(),
                Created = Convert.ToDateTime(row[CreatedColumn]),
                CustomerId = row[CustomerIdColumn] != DBNull.Value ? (Guid?)new Guid(row[CustomerIdColumn].ToString()) : null,
                Expiration = row[ExpirationColumn] != DBNull.Value
                    ? (DateTime?)Convert.ToDateTime(row[ExpirationColumn].ToString())
                    : null,
                Id = Convert.ToInt64(row[IdColumn]),
                MaxRedemptions = Convert.ToInt32(row[MaxRedemptionsColumn]),
                PromotionId = Convert.ToInt32(row[PromotionIdColumn]),
                UsedRedemptions = Convert.ToInt32(row[UsedRedemptionsColumn]),
                ValidFrom = Convert.ToDateTime(row[ValidColumn])
            };
        }
    }
}