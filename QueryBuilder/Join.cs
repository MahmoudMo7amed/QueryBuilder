using QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryBuilder
{
    public class Join
    {
        IColumn _ICoulmnLeftTable;
        JoinTypes _JoinTypes;
        IColumn _IColumnRightTable;

        public IColumn LeftColumn
        {
            get
            {
                return _ICoulmnLeftTable;
            }
        }

        public IColumn RightColumn
        {
            get
            {
                return _IColumnRightTable;
            }
        }

        public JoinTypes JoinType
        {
            get
            {
                return _JoinTypes;
            }
        }

        public Join(IColumn leftTableCol, JoinTypes joinType, IColumn rightTableCol)
        {
            if (leftTableCol == null) throw new ArgumentNullException("Left table column is null");
            if (rightTableCol == null) throw new ArgumentNullException("Right table column is null");
            this._ICoulmnLeftTable = leftTableCol;
            this._IColumnRightTable = rightTableCol;
            this._JoinTypes = joinType;
        }

        //public string getJoinBeginingFromJoinType()
        //{
        //    return string.Format("  {0} {1} on {2} = {3}"
        //                           , GetJoinString(_JoinTypes)
        //                           , getTableName(_IColumnRightTable.Container)
        //                           , _ICoulmnLeftTable.GetFullyQualifiedName()
        //                           , _IColumnRightTable.GetFullyQualifiedName()

        //     );
        //}

        //public override string ToString()
        //{
        //    string _Return = string.Empty;

        //    _Return = string.Format("{0} {1} {2} on {3} = {4}", getTableName(_ICoulmnLeftTable.Container)
        //                                , GetJoinString(_JoinTypes)
        //                                , getTableName(_IColumnRightTable.Container)
        //                                , _ICoulmnLeftTable.GetFullyQualifiedName()
        //                                , _IColumnRightTable.GetFullyQualifiedName()

        //          );
        //    return _Return;
        //}

        #region Private functions
        //string getTableName(IField ColHolder)
        //{
        //    string _Return;
        //    if (string.IsNullOrEmpty(ColHolder.Alias))
        //    {
        //        _Return = ColHolder.Name;
        //    }
        //    else
        //    {
        //        _Return = string.Format("{0} {1}", ColHolder.Name, ColHolder.Alias);
        //    }
        //    return _Return;
        //}
        //string GetJoinString(JoinTypes joinType)
        //{
        //    string _return;
        //    switch (joinType)
        //    {
        //        case JoinTypes.InnerJoin:
        //            _return = " inner join ";
        //            break;
        //        case JoinTypes.LeftOuterJoin:
        //            _return = " left outer join ";
        //            break;
        //        case JoinTypes.RightOuterJoin:
        //            _return = " left outer join ";
        //            break;
        //        case JoinTypes.FullOuterJoin:
        //            _return = " full outer join ";
        //            break;
        //        default:
        //            _return = string.Empty;
        //            break;
        //    }

        //    return Environment.NewLine + _return;
        //}
        #endregion
    }
}
