﻿using BdP_MV.Model;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class SucheViewModell : BaseNavigationViewModel
    {

        public MainController mainc;
        public Boolean nurAktiv { get; set; }
        public SearchObject suche { get; set; }

        public SucheViewModell()
        {

        }
        public SucheViewModell(MainController mainco)
        {
            mainc = mainco;
            suche = new SearchObject();
            nurAktiv = true;
        }
        public async Task<List<Mitglied>> SuchDuApp()
        {
            IsBusy = true;
            if (nurAktiv)
            {
                suche.mglStatusId = "AKTIV";
            }
            else
            {
                suche.mglStatusId = "";
            }

            List<Mitglied> mitglieder = await mainc.mitgliederController.MitgliederAbrufenBySearch(suche);
            IsBusy = false;
            return mitglieder;

        }
        public async Task<ItemDetailViewModel> mitgliedDetailsVorladen(int idMitglied, int idGruppe)

        {
            Task<List<SGB8>> task_sgb8 = mainc.mitgliederController.Sgb8Abrufen(idMitglied);
            Task<List<Ausbildung>> task_ausbildung = mainc.mitgliederController.AusbildungenAbrufen(idMitglied);
            Task<List<Taetigkeit>> task_taetigkeiten = mainc.mitgliederController.TaetigkeitenAbrufen(idMitglied);
            Task<Boolean> task_editable = mainc.groupControl.CheckPermissionForEdit(idGruppe);
            MitgliedDetails mitgliedDetails = await Task.Run(async () => await mainc.mitgliederController.MitgliedDetailsAbrufen(idMitglied, idGruppe));
            ItemDetailViewModel viewModelMitgliedDetails = new ItemDetailViewModel(mitgliedDetails, mainc);
            await Task.WhenAll(task_sgb8, task_taetigkeiten);

            viewModelMitgliedDetails.sgb8 = await task_sgb8;
            viewModelMitgliedDetails.taetigkeiten = await task_taetigkeiten;
            Task nachbarbeitung = viewModelMitgliedDetails.Nachbearbeitung();
            viewModelMitgliedDetails.isEditable = await task_editable;
            viewModelMitgliedDetails.ausbildung = await task_ausbildung;
            await nachbarbeitung;

            return viewModelMitgliedDetails;




        }
    }
}
