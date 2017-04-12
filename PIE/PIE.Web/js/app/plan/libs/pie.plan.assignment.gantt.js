(function () {
    var serviceRoot = $("#hidBaseUri").val();
    var alias = $("#hidIdentityName").val();
    var sprintId = $("#hidCurSprint").val();
    var headers = {
        'Content-Type': 'application/json', Accept: 'application/json'
    };
    var filter = "?$filter=CreateBy eq '{CreateBy}'".replace("{CreateBy}", alias);
    var request = {
        requestUri: serviceRoot + 'Plans' + filter,
        method: 'GET',
        headers: headers
    };

    var bindGant = function () {
        var pageNum = 15;
        var gantData = [];
        odatajs.oData.request(
            request,
            function (data, response) {
                console.log(">>odatajs.oData.request - get - callback -succeed");
                console.log(data);
                console.log(response);

                $.each(data.value, function (index, plan) {
                    var item = {
                    };
                    item.name = plan.Title;
                    item.values = [];
                    var valueItem = {};
                    valueItem.id = plan.ID;
                    valueItem.from = plan.StartDate;
                    valueItem.to = plan.EndDate;
                    valueItem.desc = "{Id}:{Name}".replace("{Id}", plan.ID).replace("{Name}", plan.Title);
                    valueItem.label = plan.Title;
                    item.values.push(valueItem);
                    gantData.push(item);
                });

                if (gantData.length == 0) {
                    return;
                }

                for (i = 0; i <= gantData.length % pageNum; i++) {
                    var item = {};
                    item.values = [];
                    var valueItem = {};
                    gantData.push(item);
                }
                
                $(".gantt").gantt({
                    source: gantData,
                    itemsPerPage: pageNum,
                    navigate: 'scroll',
                    scale: 'days',
                    maxScale: 'weeks',
                    minScale: 'hours'
                });

                return gantData;
            },
            function (err) {
                console.error(err);
            }
        );
    }
    bindGant();
    window.bindGant = bindGant;
})();