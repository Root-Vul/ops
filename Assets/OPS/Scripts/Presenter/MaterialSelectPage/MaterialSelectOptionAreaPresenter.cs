﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OPS.Model;
using Zenject;
using System.Linq;

namespace OPS.Presenter
{

	public class MaterialSelectOptionAreaPresenter : MonoBehaviour
	{
		[Inject]
		UserMixCandidateMaterialOptionDB _userMixCandidateMaterialOptionDB;

		public Text culumnText;

		int MaterialOptionId {get;set;}

		UserMixCandidateMaterialOptionModel _userMixCandidateMaterialOptionModel;

		public void SetText(string text)
		{
			culumnText.text = text;
		}

		public void Recovery(UserMixCandidateMaterialOptionModel userMixCandidateMaterialOptionModel)
		{
			_userMixCandidateMaterialOptionModel = userMixCandidateMaterialOptionModel;
			culumnText.text = _userMixCandidateMaterialOptionModel.MasterOptionModel.name.Value;
		}

		public void SetOption(MasterOptionModel masterOptionModel, UserMixCandidateMaterialModel userMixCandidateMaterialModel)
		{
			var newUserMixCandidateMaterialOptionModel = _userMixCandidateMaterialOptionDB.New();
			newUserMixCandidateMaterialOptionModel.master_option_id.Value = masterOptionModel.id.Value;
			newUserMixCandidateMaterialOptionModel.material_sort_index.Value = userMixCandidateMaterialModel.sort_index.Value;
			newUserMixCandidateMaterialOptionModel.sort_index.Value = _userMixCandidateMaterialOptionDB.Where("material_sort_index", userMixCandidateMaterialModel.sort_index.Value.ToString()).Count;
			var _userMixCandidateMaterialOptionModel = _userMixCandidateMaterialOptionDB.Save(newUserMixCandidateMaterialOptionModel).First().Value;
			culumnText.text = _userMixCandidateMaterialOptionModel.MasterOptionModel.name.Value;
		}

		public void OnSelectOption()
		{			
		}

        public class Factory : PlaceholderFactory<MaterialSelectOptionAreaPresenter>
		{
		}
	}

}
