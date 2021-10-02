tableEditor
    .controller('dml', function (logger, svcDML, svcVoc, $scope) {
        dml = this;
        dml.rowsList = [];
        dml.tableCode = null;
        dml.show = {};
        dml.searchString = "";
        dml.ExtVoc = {};

        dml.click = function () {
            console.log(this.tablesList);
        }

        dml.InitTableCode = function (tableCode) {
            dml.tableCode = tableCode;
            dml.LoadRows();
            dml.LoadVocabularies();
        }

        dml.GetVisibleFields = function () {
            var ans = [];
            for (var fieldName in dml.show) {
                if (dml.show[fieldName]) {
                    ans.push(fieldName);
                }
            }
            return ans;
        }

        dml.LoadVocabularies = function () {
            svcVoc.GetVocList().then(function (ajaxData) {
                dml.vocList = {}
                for (var vodIndex in ajaxData) {
                    var vocId = ajaxData[vodIndex].id;
                    var vocTerms = ajaxData[vodIndex].termsList;
                    dml.vocList[vocId] = vocTerms;
                }
                $scope.$apply();
            });
        }

        dml.LoadRows = function () {
            var fieldsList = dml.GetVisibleFields();
            svcDML.GetRows(dml.tableCode, fieldsList).then(function (data) {
                dml.rowsList = data;
                logger.log("tables loaded");
                $scope.$apply();
            })
        }
        dml.StartEditVoc = function (row, fieldName, dictID) {
            if (!row.IsEditing) {
                row.IsEditing = {};
            }
            row.IsEditing[fieldName] = true;
        }

        dml.StartEditExtVoc = function (row, fieldName, extUrl, extPath, termId, termName) {
            if (!row.IsEditing) {
                row.IsEditing = {};
            }
            row.IsEditing[fieldName] = true;

            if (!dml.ExtVoc[fieldName]) {
                svcVoc.LoadExternalVocabulary(extUrl, extPath, termId, termName).then(function (termsList) {
                    dml.ExtVoc[fieldName] = termsList;
                    logger.log("ext voc loaded");
                    $scope.$apply();
                })
            }
        }



        dml.AddRow = function () {
            dml.rowsList.push({ IsNew: true });
        }

        dml.SaveRow = function (row) {
            svcDML.SaveRow(dml.tableCode, row._id, row).then(function (data) {
                logger.log("Row saved");
                row.dirty = false;
                row.IsNew = false;
                $scope.$apply();
            })
        }

        dml.LoadRows();
    })