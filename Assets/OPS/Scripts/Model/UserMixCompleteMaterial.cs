using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

namespace OPS.Model
{

    public class UserMixCompleteMaterialDB : BaseSqliteModel<UserMixCompleteMaterialModel>
    {
        public override string DbName { get { return "user.sqlite3"; } }

        public override string TableName { get { return "user_mix_complete_materials"; } }

        [Inject]
        public MasterOptionDB _masterOptionDB = null;

        [Inject]
        public UserMixDB _userMixDB = null;

        protected override UserMixCompleteMaterialModel DataRow2Model(DataRow DataRow)
        {
            var model = new UserMixCompleteMaterialModel();
            model.SetDB(this);
            model.id.Value = (int)DataRow["id"];
            model.user_mix_id.Value = (int)DataRow["user_mix_id"];
            model.select_agenda.Value = (int)DataRow["select_agenda"];
            model.master_option_id.Value = (int)DataRow["master_option_id"];
            model.rate.Value = (double)DataRow["rate"];
            return model;
        }

        protected override DataRow Model2DataRow(UserMixCompleteMaterialModel model)
        {
            var dataRow = new DataRow();
            dataRow["id"] = model.id.Value;
            dataRow["user_mix_id"] = model.user_mix_id.Value;
            dataRow["select_agenda"] = model.select_agenda.Value;
            dataRow["master_option_id"] = model.master_option_id.Value;
            dataRow["rate"] = model.rate.Value;
            return dataRow;
        }
    }

    public class UserMixCompleteMaterialModel
    {
        public Dictionary<int, double> ExtraRateTable = new Dictionary<int, double>()
        {
            {0, 1}, // 合成後スロット数0は本来存在しない。エラー回避のため。
            {1, 1},
            {2, 0.9},
            {3, 0.85},
            {4, 0.7},
            {5, 0.6},
            {6, 0.55},
            {7, 0.4},
            {8, 0.3}
        };

        UserMixCompleteMaterialDB _userMixCompleteMaterialDB;

        public void SetDB(UserMixCompleteMaterialDB userMixCompleteMaterialDB)
        {
            _userMixCompleteMaterialDB = userMixCompleteMaterialDB;
        }

        public IntReactiveProperty id = new IntReactiveProperty();
        public IntReactiveProperty user_mix_id = new IntReactiveProperty();
        public IntReactiveProperty select_agenda = new IntReactiveProperty();
        public IntReactiveProperty master_option_id = new IntReactiveProperty();
        public DoubleReactiveProperty rate = new DoubleReactiveProperty();

        public MasterOptionModel MasterOptionModel
        {
            get { return _userMixCompleteMaterialDB._masterOptionDB.Id(master_option_id.Value).First().Value; }
        }

        public UserMixModel UserMixModel
        {
            get { return _userMixCompleteMaterialDB._userMixDB.Id(user_mix_id.Value).First().Value; }
        }

        public double IncludeExtraRate()
        {
            if (!IsExtraSlot())
            {
                return rate.Value;
            }
            return Math.Round(rate.Value * ExtraRateTable[UserMixModel.UserMixCompleteMaterialSelectAgendaModels.Count()], MidpointRounding.AwayFromZero);
        }

        public bool IsExtraSlot()
        {
            var bodyOptionCount = UserMixModel.BodyUserMixCandidateMaterialModel.OptionCount();
            var completeOptionCount = UserMixModel.UserMixCompleteMaterialSelectAgendaModels.Count();
            return bodyOptionCount < completeOptionCount;
        }

        public bool SelectAgenda()
        {
            var userMixAgendas = UserMixModel.UserMixCompleteMaterialSelectAgendaModels;
            foreach (var userMixAgenda in userMixAgendas)
            {
                if (userMixAgenda.Value != this && MasterOptionModel.category_id.Value == userMixAgenda.Value.MasterOptionModel.category_id.Value)
                {
                    userMixAgenda.Value.select_agenda.Value = 0;
                    select_agenda.Value = 1;
                    _userMixCompleteMaterialDB.Save(userMixAgenda.Value);
                    _userMixCompleteMaterialDB.Save(this);
                    return true;
                }
            }
            if (userMixAgendas.Count() > UserMixModel.BodyUserMixCandidateMaterialModel.OptionCount()) return false;
            select_agenda.Value = 1;
            _userMixCompleteMaterialDB.Save(this);
            return true;
        }
    }

}