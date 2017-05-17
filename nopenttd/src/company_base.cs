/* $Id$ */

/*
 * This file is part of OpenTTD.
 * OpenTTD is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, version 2.
 * OpenTTD is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with OpenTTD. If not, see <http://www.gnu.org/licenses/>.
 */

/** @file company_base.h Definition of stuff that is very close to a company, like the company struct itself. */

using System.Collections.Generic;
using System.Linq;
using Nopenttd.src;
using Nopenttd.Tiles;

namespace Nopenttd
{

/** Statistics about the economy. */
    public struct CompanyEconomyEntry
    {
        /// The amount of income.
        public Money income;

        /// The amount of expenses.
        public Money expenses;

        /// The amount of delivered cargo.
        public CargoArray delivered_cargo;

        /// Company score (scale 0-1000)
        public int performance_history;

        /// The value of the company.
        public Money company_value;
    };

    public class CompanyInfrastructure
    {
        /// Count of company owned track bits for each road type.
        public uint[] road = new uint[(int) RoadType.ROADTYPE_END];

        /// Count of company owned signals.
        public uint signal;

        /// Count of company owned track bits for each rail type.
        public uint[] rail = new uint[(int) RailType.RAILTYPE_END];

        /// Count of company owned track bits for canals.
        public uint water;

        /// Count of company owned station tiles.
        public uint station;

        /// Count of company owned airports.
        public uint airport;

        /** Get total sum of all owned track bits. */
        public uint GetRailTotal()
        {
            uint total = 0;
            for (var rt = RailType.RAILTYPE_BEGIN; rt < RailType.RAILTYPE_END; rt++)
            {
                total += rail[(int)rt];
            }
            return total;
        }

/** Statically loadable part of Company pool item */
        public class CompanyProperties
        {
            /// Parameter of #name_1.
            public uint name_2;

            /// Name of the company if the user did not change it.
            public ushort name_1;

            /// Name of the company if the user changed it.
            public string name;

            /// Name of the president if the user did not change it.
            public ushort president_name_1;

            /// Parameter of #president_name_1
            public uint president_name_2;

            /// Name of the president if the user changed it.
            public string president_name;

            /// Face description of the president.
            public CompanyManagerFace face;

            /// Money owned by the company.
            public Money money;

            /// Fraction of money of the company, too small to represent in #money.
            public byte money_fraction;

            /// Amount of money borrowed from the bank.
            public Money current_loan;

            /// Company colour.
            public byte colour;

            /// Rail types available to the company.
            public RailTypes avail_railtypes;

            /// Number of quarters that the company is not allowed to get new exclusive engine previews (see CompaniesGenStatistics).
            public byte block_preview;

            /// Northern tile of HQ; #INVALID_TILE when there is none.
            public TileIndex location_of_HQ;

            /// Coordinate of the last build thing by this company.
            public TileIndex last_build_coordinate;

            /// Owners of the 4 shares of the company. #INVALID_OWNER if nobody has bought them yet.
            public OwnerByte share_owners[4];

            /// Year of starting the company.
            public Year inaugurated_year;

            /// Number of months that the company is unable to pay its debts
            public byte months_of_bankruptcy;

            /// which companies were asked about buying it?
            public CompanyMask bankrupt_asked;

            /// If bigger than \c 0, amount of time to wait for an answer on an offer to buy this company.
            public short bankrupt_timeout;

            public Money bankrupt_value;

            /// Amount of tileheights we can (still) terraform (times 65536).
            public uint terraform_limit;

            /// Amount of tiles we can (still) clear (times 65536).
            public uint clear_limit;

            /// Amount of trees we can (still) plant (times 65536).
            public uint tree_limit;

            /**
             * If \c true, the company is (also) controlled by the computer (a NoAI program).
             * @note It is possible that the user is also participating in such a company.
             */
            public bool is_ai;

            /// Expenses of the company for the last three years, in every #Expenses category.
            public Money[][] yearly_expenses = new Money[3][];

            /// Economic data of the company of this quarter.
            public CompanyEconomyEntry cur_economy;

            /// Economic data of the company of the last #MAX_HISTORY_QUARTERS quarters.
            public CompanyEconomyEntry[] old_economy = new CompanyEconomyEntry[OwnerConstants.MAX_HISTORY_QUARTERS];

            public byte num_valid_stat_ent;

            /// Number of valid statistical entries in #old_economy.
            public CompanyProperties()
            {
                for (var i = 0; i < yearly_expenses.Length; i++)
                {
                    yearly_expenses[i] = new Money[(int) ExpensesType.EXPENSES_END];
                }
            }

        };

        public class Company : CompanyProperties
        {


            public static readonly List<Company> _company_pool = new List<Company>((int) Owner.MAX_COMPANIES);

            public Company(ushort name_1 = 0, bool is_ai = false)
            {
            }

            public Livery[] livery = new Livery[(int)LiveryScheme.LS_END];

            /// Road types available to this company.
            public RoadTypes avail_roadtypes;

            public AIInstance ai_instance;
            public AIInfo ai_info;

            /// Engine renewals of this company.
            public EngineRenewList engine_renew_list;

            /// settings specific for each company
            public CompanySettings settings;

            /// NOSAVE: Statistics for the ALL_GROUP group.
            public GroupStatistics[] group_all = new GroupStatistics[(int) VehicleType.VEH_COMPANY_END];

            /// NOSAVE: Statistics for the DEFAULT_GROUP group.
            public GroupStatistics[] group_default = new GroupStatistics[(int) VehicleType.VEH_COMPANY_END];

            CompanyInfrastructure infrastructure;

            /// NOSAVE: Counts of company owned infrastructure.

            /**
             * Is this company a valid company, controlled by the computer (a NoAI program)?
             * @param index Index in the pool.
             * @return \c true if it is a valid, computer controlled company, else \c false.
             */
            public static bool IsValidAiID(int index)
            {
                var c = index < _company_pool.Count ? _company_pool[index] : null;
                return c != null && c.is_ai;
            }

            /**
             * Is this company a valid company, not controlled by a NoAI program?
             * @param index Index in the pool.
             * @return \c true if it is a valid, human controlled company, else \c false.
             * @note If you know that \a index refers to a valid company, you can use #IsHumanID() instead.
             */
            public static bool IsValidHumanID(int index)
            {
                var c = index < _company_pool.Count ? _company_pool[index] : null;
                return c != null && !c.is_ai;
            }

            /**
             * Is this company a company not controlled by a NoAI program?
             * @param index Index in the pool.
             * @return \c true if it is a human controlled company, else \c false.
             * @pre \a index must be a valid CompanyID.
             * @note If you don't know whether \a index refers to a valid company, you should use #IsValidHumanID() instead.
             */
            public static bool IsHumanID(int index)
            {
                return !_company_pool[index].is_ai;
            }

            //static void PostDestructor(int index);
            public static IEnumerable<Company> FOR_ALL_COMPANIES_FROM(int index = 0)
            {
                var len = Company._company_pool.Count;
                for (var i = index; i < len; i++)
                {
                    var item = Company._company_pool[i];
                    if (item != null)
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}