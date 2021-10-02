tableEditor
    .controller('ctrlVoc', function (logger, svcVoc, $scope) {
        ctrlVoc = this;
        ctrlVoc.vocList = [];
        ctrlVoc.termsList = [];

        ctrlVoc.selectedVocabulary = null;
        ctrlVoc.selectedTerm = null;



        ctrlVoc.Init = function () {
            ctrlVoc.LoadVocabularyList();
        }

        //// Vocabulary

        ctrlVoc.LoadVocabularyList = function () {
            ctrlVoc.vocList = [];
            ctrlVoc.termsList = [];
            ctrlVoc.selectedVocabulary = null;
            ctrlVoc.selectedTerm = null;

            svcVoc.GetVocList().then(function (ajaxData) {
                ctrlVoc.vocList = ajaxData;
                $scope.$apply();
            })
        }

        ctrlVoc.SelectVocabulary = function (voc) {
            if (ctrlVoc.selectedVocabulary == voc) return;

            ctrlVoc.selectedTerm = null;
            ctrlVoc.termsList = null;

            ctrlVoc.selectedVocabulary = voc;
            svcVoc.GetVoc(voc.id).then((ajaxData) => {
                ctrlVoc.selectedVocabularyData = ajaxData;
                if (ctrlVoc.selectedVocabulary.termsList == null) ctrlVoc.selectedVocabulary.termsList = [];
                ctrlVoc.termsList = ctrlVoc.selectedVocabulary.termsList;
                //     return svcVoc.GetVocTerms(voc.id);
                // }).then(function (ajaxData) {
                //     ctrlVoc.termsList = ajaxData;
                $scope.$apply();
            });

        }

        ctrlVoc.AddVocabulary = function () {
            var newVoc = { id: "", name: "", IsNew: true, IsEditing: true };
            ctrlVoc.vocList.push(newVoc);
        }

        ctrlVoc.SaveVocabulary = function (voc) {
            svcVoc.SaveVoc(voc.id, voc).then((ajaxData) => {
                voc.IsDirty = false;
                voc.IsNew = false;
                ctrlVoc.SelectVocabulary(voc);
                $scope.$apply();
            });
        }

        ctrlVoc.MarkSelectedVocabulary = function () {
            if (ctrlVoc.selectedVocabulary != null) {
                ctrlVoc.selectedVocabulary.IsDirty = true;
            }
        }


        //// Terms


        ctrlVoc.SelectTerm = function (term) {
            if (ctrlVoc.selectedTerm == term) return;

            ctrlVoc.selectedTerm = term;
        }

        ctrlVoc.AddTerm = function () {
            var newTerm = { IsNew: true, IsEditing: true };
            ctrlVoc.termsList.push(newTerm);
            ctrlVoc.SelectTerm(newTerm);
            ctrlVoc.MarkSelectedVocabulary();
        };

        ctrlVoc.RemoveTerm = function (term) {
            var indx = this.termsList.indexOf(term);
            if (indx >= 0) {
                this.termsList.splice(indx, 1);
                ctrlVoc.MarkSelectedVocabulary();
            }
        }



        ctrlVoc.Init();
    })