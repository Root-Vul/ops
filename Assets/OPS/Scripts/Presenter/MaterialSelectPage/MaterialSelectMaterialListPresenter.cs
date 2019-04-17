﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OPS.Presenter
{
    public class MaterialSelectMaterialListPresenter : MonoBehaviour
    {
        [Inject]
        MaterialSelectOptionListPresenter.Factory _optionListPresenterFactory;

        [SerializeField]
        MaterialSelectPagePresenter _materialSelectPagePresenter;

        [SerializeField]
        GameObject _addOptionListObject;

        public void Recovery()
        {
            var userMixCandidateMaterialModels = _materialSelectPagePresenter.UserMixModel.UserMixCandidateMaterialModel;
            foreach (var userMixCandidateMaterialModel in userMixCandidateMaterialModels)
            {
                var cpyOptionList = _optionListPresenterFactory.Create();
                cpyOptionList.Recovery(userMixCandidateMaterialModel.Value);
                cpyOptionList.transform.SetParent(_addOptionListObject.transform, false);
                cpyOptionList.transform.SetAsFirstSibling();
            }
        }

        public void OnAddList()
        {
            var cpyOptionList = _optionListPresenterFactory.Create();
            cpyOptionList.AddSetup(_materialSelectPagePresenter.UserMixModel);
            cpyOptionList.transform.SetParent(_addOptionListObject.transform, false);
        }

    }
}