﻿using System;
using UnityEngine;
using Zenject;
using OPS.Presenter;

public class PageManagerInstaller : MonoInstaller<PageManagerInstaller>
{
    [SerializeField]
    GameObject _mainCanvas;

    [SerializeField]
    GameObject _selectOptionList;

    [SerializeField]
    GameObject _optionArea;

    [SerializeField]
    GameObject _completeSelectPage;

    [SerializeField]
    GameObject _completeSelectListButton;

    [SerializeField]
    GameObject _materialSelectPage;

    [SerializeField]
    GameObject _optionSelectPage;

    [SerializeField]
    GameObject _mixPage;

    public override void InstallBindings()
    {
        Container.Bind<PageManager>().FromComponentOn(_mainCanvas).AsSingle();
        Container.BindFactory<MaterialSelectOptionListPresenter, MaterialSelectOptionListPresenter.Factory>().FromComponentInNewPrefab(_selectOptionList);
        Container.BindFactory<MaterialSelectOptionAreaPresenter, MaterialSelectOptionAreaPresenter.Factory>().FromComponentInNewPrefab(_optionArea);
        Container.BindFactory<CompleteSelectPagePresenter, CompleteSelectPagePresenter.Factory>().FromComponentInNewPrefab(_completeSelectPage);
        Container.BindFactory<CompleteSelectListButtonPresenter, CompleteSelectListButtonPresenter.Factory>().FromComponentInNewPrefab(_completeSelectListButton);
        Container.BindFactory<MaterialSelectPagePresenter, MaterialSelectPagePresenter.Factory>().FromComponentInNewPrefab(_materialSelectPage);
        Container.BindFactory<OptionSelectPagePresenter, OptionSelectPagePresenter.Factory>().FromComponentInNewPrefab(_optionSelectPage);
        Container.BindFactory<MixPagePresenter, MixPagePresenter.Factory>().FromComponentInNewPrefab(_mixPage);
    }
}