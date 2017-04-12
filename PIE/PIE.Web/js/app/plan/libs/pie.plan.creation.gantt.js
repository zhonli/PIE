(function () {
    var serviceRoot = $("#hidBaseUri").val();
    var alias = $("#hidIdentityName").val();
    var sprintId = $("#hidCurSprint").val();
    var headers = {
        'Content-Type': 'application/json', Accept: 'application/json'
    };
    var filter = "?$filter=CreateBy eq '{CreateBy}' &$orderby=StartDate asc".replace("{CreateBy}", alias);
    var request = {
        requestUri: serviceRoot + 'Plans' + filter,
        method: 'GET',
        headers: headers
    };

    var bindGant = function () {
        
        var gantData = [];
        odatajs.oData.request(
            request,
            function (data, response) {
                console.log(">>odatajs.oData.request - get - callback -succeed");
                console.log(data);
                console.log(response);
                var pageNum = data.value.length;
                $.each(data.value, function (index, plan) {
                    var item = {
                    };
                    item.name = plan.Title;
                    item.values = [];
                    var valueItem = {};
                    valueItem.id = plan.ID;
                    valueItem.from = plan.StartDate;
                    valueItem.to = plan.EndDate;
                    //valueItem.desc = "{Id}:{Name}".replace("{Id}", plan.ID).replace("{Name}", plan.Title);
                    valueItem.label = plan.Title;
                    item.values.push(valueItem);
                    gantData.push(item);
                });

                if (gantData.length == 0) {
                    
                    $(".gantt").hide();
                    return;
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