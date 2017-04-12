function assignment(curPlanId) {
    console.log(">>>>>>pie.pm.plan.assignment.js");

    var serviceRoot = $("#hidBaseUri").val();
    var curUserAlias = $("#hidIdentityName").val();

    console.log(">>val: hidCurPlanId");
    console.log($("#hidCurPlanId").val());

    var disableGrid = false;
    if (!curPlanId) {
        curPlanId = -1;
        disableGrid = true;
    }

    console.log(">> curPlanId, disableGrid");
    console.log(curPlanId);
    console.log(disableGrid);

    var theme = 'bootstrap';

    var headers = { 'Content-Type': 'application/json', Accept: 'application/json' };
    var filter = "$expand=Resource&$filter=PlanID eq {PlanID}".replace("{PlanID}", curPlanId);
    var filterU = '';
    // prepare the datasource for the data
    var source =
    {
        datatype: 'json',
        url: serviceRoot + 'Assignments',
        datafields:
        [
            { name: 'ID', type: 'int' },
            { name: 'ResourceID', type: 'int', map: 'Resource>ResourceID' },
            { name: 'ResourceAlias', type: 'string', map: 'Resource>Alias' },
            { name: 'Units', type: 'float' }
        ],
        sortcolumn: 'ID',
        sortdirection: 'desc',

        sort: function (column, direction) {
            console.log(">>sort: column, direction");
            console.log(cloumn);
            console.log(direction);
        },

        filter: function (filters, recordsArray) {
            console.log(">>filter: filters, recordsArray");
            console.Log(filters);
            console.Log(recordsArray);
        },

        addrow: function (rowid, rowdata, position, commit) {
            rowdata.ResourceID = rowdata.ResourceAlias.value;
            rowdata.ResourceAlias = rowdata.ResourceAlias.label;
            console.log(">>addrow :: rowid, rowdata, position, commit");
            console.log(rowid);
            console.log(rowdata);
            console.log(position);
            console.log(commit);

            var assignment = {};
            assignment.PlanID = curPlanId;
            assignment.ResourceID = rowdata.ResourceID;
            assignment.Units = rowdata.Units;
            assignment.CreateBy = curUserAlias;

            var request = {
                requestUri: serviceRoot + 'Assignments',
                method: 'POST',
                headers: headers,
                data: assignment
            };

            odatajs.oData.request(
                request,
                function (data, response) {
                    console.log(">>odatajs.oData.request -succeed:: data");
                    console.log(data);

                    rowdata.ID = data.ID;
                    commit(true);
                },
                function (err) {
                    console.error(err);
                }
            );
        },

        updaterow: function (rowid, rowdata, commit) {
            console.log(">>updaterow:: odatajs.oData.request -succeed:: data");
            console.log(rowid);
            console.log(rowdata);

            var assignmentId = rowdata.ID;
            var assignment = {};
            assignment.PlanID = curPlanId;
            assignment.ResourceID = rowdata.ResourceID;
            assignment.Units = rowdata.Units;
            assignment.LastModifiedBy = curUserAlias;

            var request = {
                requestUri: serviceRoot + 'Assignments(' + assignmentId + ')',
                method: 'PATCH',
                headers: headers,
                data: assignment
            };

            odatajs.oData.request(
                request,
                function (data, response) {
                    console.log(">>updaterow:: odatajs.oData.request -succeed:: data");
                    console.log(data);

                    commit(true);
                },
                function (err) {
                    console.error(err);
                }
            );
        },

        deleterow: function (rowid, commit) {
            console.log(">>deleterow :: assignmentId, commit");
            console.log(rowid);
            console.log(commit);

            var rowdata = $("#jqxgrid").jqxGrid('getrowdatabyid', rowid);
            var assignmentId = rowdata.ID;

            var request = {
                requestUri: serviceRoot + 'Assignments(' + assignmentId + ')',
                method: 'DELETE',
                headers: headers
            };

            odatajs.oData.request(
                request,
                function (data, response) {
                    console.log(">>deleterow:: odatajs.oData.request - delete -succeed");
                    console.log(data);

                    commit(true);
                    //TODO faild to delete.
                },
                function (err) {
                    console.error(err);
                }
            );
        },
    };

    // prepare the dataAdapter
    var dataAdapter = new $.jqx.dataAdapter(source, {
        contentType: 'application/json; charset=utf-8',

        beforeSend: function (jqXHR, settings) {
            //A pre-request callback function that can be used to modify the jqXHR
            console.log('>>dataAdapter.beforeSend: preSettings, settings');
            console.log(settings);
            settings.url = settings.url.split('?')[0] + '?' + filter + filterU;
            console.log(settings);
        },
        beforeLoadComplete: function (records) {
            console.log(">>dataAdapter.beforeLoadComplete: records")
            console.log(records);
        },
        downloadComplete: function (edata, textStatus, jqXHR) {
            console.log(">>dataAdapter.downloadComplete:: edata, textStatus, jqXHR");
            console.log(edata);
            console.log(textStatus);
            console.log(jqXHR);
        },
        loadComplete: function (records) {
            console.log(">>dataAdapter.loadComplete :: records")
            console.log(records);
        },
        loadError: function (jqXHR, status, error) {
            console.group(">>dataAdapter.loadError :: jqXHR, status, error");
            console.log(jqXHR);
            console.log(status);
            console.log(error);
        }
    });
    var resources;
    var getResources = function (name) {
        var resouceSource =
        {
            datatype: "json",
            datafields: [
                { name: 'ResourceID', type: 'int' },
                { name: 'Alias', type: 'string' },
                { name: 'DisplayName', type: 'string' },
                { name: 'Role', type: 'string' }
            ],
            url: serviceRoot + "Resources",
            async: false
        };

        var fields = new Array();
        fields.push(name);
        var resourceDataAdapter = new $.jqx.dataAdapter(resouceSource, {
            autoBind: true,
            autoSort: true,
            uniqueDataFields: fields,
            autoSortField: name
        });
        resources = resourceDataAdapter.records;
        console.log(">>getResources:: resources");
        console.log(resources);
        return resourceDataAdapter.records;
    }

    // prepare the jqxGrid
    var currentCellValue;
    var imagerenderer = function (row, datafield, value) {

        return '<div  style="text-align:center"><img style="margin-top:5px" src="../img/close_black.png"/></div>';
    }
    $("#jqxgrid").jqxGrid({
        theme: theme,
        width: 336,
        
        autoheight: true,
        source: dataAdapter,
        editable: true,
        pageable: false,
        disabled: disableGrid,
        enabletooltips: true,
        showeverpresentrow: true,
        everpresentrowposition: "top",
        everpresentrowactionsmode: "columns",
        selectionmode: 'singlecell',
        editmode: 'selectedcell',
        columns: [
          {
              text: 'Resource Alias', datafield: 'ResourceAlias', columntype: 'textbox', width: 216, editable: false,
              createEverPresentRowWidget: function (datafield, htmlElement, popup, addCallback) {
                  $(htmlElement).width($(htmlElement).width() + 1);
                  var inputTag = $("<input name='everPresent' style='border: none;'/>").appendTo(htmlElement);

                  inputTag.jqxInput({ theme: theme, popupZIndex: 99999999, placeHolder: "Enter Resource Alias", source: getResources("Alias"), displayMember: 'Alias', valueMember: 'ResourceID', width: '100%', height: 30 });
                  inputTag.on('select', function (event) {
                      if (event.args) {
                          var item = event.args.item;
                          if (item) {
                              console.log(">>Resource Alias:: select");
                              console.log(item);
                          }
                      }
                  });
                  $(document).on('keydown.name', function (event) {
                      if (event.keyCode == 13) {
                          console.log(">>Resource Alias::Enter::event, inputTag[0], addCallback");
                          console.log(event);
                          console.log(inputTag[0]);
                          console.log(addCallback);
                          if (event.target === inputTag[0]) {
                              addCallback();
                          }
                      }
                  });
                  return inputTag;
              },
              initEverPresentRowWidget: function (datafield, htmlElement) {
              },
              validateEverPresentRowWidgetValue: function (datafield, value, rowValues) {
                  console.log(">>validateEverPresentRowWidgetValue::datafield, value, rowValues");
                  console.log(datafield);
                  console.log(value);
                  console.log(rowValues);
                  var inputAlias = this.addnewrowwidget[0].value;
                  var matched = false;
                  var existed = false;
                  for (var i = 0; i < resources.length; i++) {
                      if (resources[i].Alias == value.label && resources[i].Alias == inputAlias) {
                          matched = true;
                          break;
                      }
                  }
                  for (var i = 0; i < dataAdapter.records.length; i++) {
                      if (dataAdapter.records[i].ResourceAlias == value.label) {
                          existed = true;
                          break;
                      }
                  }

                  if (!matched) {
                      return { message: "Can't match resource alias in resource pool", result: false };
                  }
                  if (existed) {
                      return { message: "{Alias} has been assigned in list".replace("{Alias}", value.label), result: false };
                  }
                  return true;
              },
              getEverPresentRowWidgetValue: function (datafield, htmlElement, validate) {
                  var value = htmlElement.val();
                  return value;
              },
              resetEverPresentRowWidgetValue: function (datafield, htmlElement) {
                  htmlElement.val("");
              }
          },
          {
              text: 'Units', datafield: 'Units', columntype: 'numberinput', cellsalign: 'right', align: 'right', cellsformat: 'pn', width: 80, editable: true,
              createEverPresentRowWidget: function (datafield, htmlElement, popup, addCallback) {
                  $(htmlElement).width($(htmlElement).width() + 1);

                  var inputTag = $("<div name='everPresent' style='border: none;'></div>").appendTo(htmlElement);
                  inputTag.jqxNumberInput({ theme: theme, width: '100%', height: 30, digits: 3, inputMode: 'simple', decimalDigits: 0,  symbolPosition: 'right', symbol: '%', spinButtons: true });
                  $(document).on('keydown.qty', function (event) {
                      if (event.keyCode == 13) {
                          if (event.target === inputTag[0]) {
                              console.log(">>Units::Enter::inputTag");
                              console.log(inputTag);
                              addCallback();
                          }
                          else if ($(event.target).ischildof(inputTag)) {
                              addCallback();
                          }
                      }
                  });
                  return inputTag;
              },
              initEverPresentRowWidget: function (datafield, htmlElement) {

              },
              validateEverPresentRowWidgetValue: function (datafield, value, rowValues) {
                  if (parseInt(value) < 0) {
                      return { message: "Entered value should be positive number", result: false };
                  }
                  return true;
              },
              getEverPresentRowWidgetValue: function (datafield, htmlElement, validate) {
                  var value = htmlElement.val();
                  if (value == "") value = 0;
                  return parseInt(value);
              },
              resetEverPresentRowWidgetValue: function (datafield, htmlElement) {
                  htmlElement.val("");
              }
          },
          {
              text: 'Ops', datafield: 'ID', width: 40, cellsalign: 'center', align: 'center', editable: false, cellsrenderer: imagerenderer,
              createEverPresentRowWidget: function (datafield, htmlElement, popup, addCallback) {
                  $(htmlElement).width($(htmlElement).width() + 1);
                  $(htmlElement).attr("name", "opsEverPresent");
                  var inputTag = $("<div name='everPresent' style='border: none;'></div>").appendTo(htmlElement);
                  inputTag.jqxButton({ width: '100%', height: 30 });

                  $(document).on('keydown.qty', function (event) {
                      if (event.keyCode == 13) {
                          if (event.target === inputTag[0]) {
                              console.log(">>Ops::Enter::inputTag");
                              console.log(inputTag);
                              addCallback();
                          }
                          else if ($(event.target).ischildof(inputTag)) {
                              addCallback();
                          }
                      }
                  });
                  return inputTag;
              }
          }
        ]
    });
    

    //bind the Events
    $("#jqxgrid").bind('cellbeginedit', function (event) {
        console.log(">>on cellbeginedit::event");
        console.log(event);

        var args = event.args;
        var columnDataField = args.datafield;
        var rowIndex = args.rowindex;
        currentCellValue = args.value;
    });

    $("#jqxgrid").bind('cellendedit', function (event) {
        console.log(">>on cellendedit::event");
        console.log(event);

        var args = event.args;
        var columnDataField = args.datafield;
        var rowIndex = args.rowindex;
        var cellValue = args.value;
        var oldValue = args.oldvalue;
    });

    $("#jqxgrid").bind("pagechanged", function (event) {
        console.log(">>on pagechanged::event");
        console.log(event);

        var args = event.args;
        console.log(args.pagenum);
        console.log(args.pagesize);

        // get page information.
        var paginginformation = $("#jqxgrid").jqxGrid('getpaginginformation');
        console.log(paginginformation.pagenum);
        console.log(paginginformation.pagesize);
        console.log(paginginformation.pagescount);
    });

    $("#jqxgrid").bind("pagesizechanged", function (event) {
        console.log(">>on pagesizechanged::event");
        console.log(event);

        var args = event.args;
        console.log(args.pagenum);
        console.log(args.pagesize);
        console.log(args.oldpagesize);

        // get page information.
        var paginginformation = $("#jqxgrid").jqxGrid('getpaginginformation');

        console.log(paginginformation.pagenum);
        console.log(paginginformation.pagesize);
        console.log(paginginformation.pagescount);
    });

    $("#jqxgrid").on("celldoubleclick", function (event) {
        // event arguments.
        var args = event.args;
        var dataField = args.datafield;

        if (dataField != 'ID')
            return;

        var args = event.args;
        var curAssignment = args.row.bounddata;
        console.log(">>cellclick:: curAssignment");
        console.log(curAssignment);

        var rowBoundIndex = args.rowindex;
        var rowId = $("#jqxgrid").jqxGrid('getrowid', rowBoundIndex);
        var commit = $("#jqxgrid").jqxGrid('deleterow', rowId);
    });

    $("#jqxgrid").on('cellvaluechanged', function (event) {
        // event arguments.
        var args = event.args;
        var dataField = args.datafield;
        if (dataField != 'Units')
            return;

        console.log(">>on cellvaluechanged::event");
        console.log(event);

    });

    $("#jqxgrid").blur();
}