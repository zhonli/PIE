angular.module('pie').factory('BCService', function () {
    console.log(">>>>BCService");

    //defined BillingCycle object
    var BillingCycle = function (date) {
        if (typeof date == 'string') {
            date = StringToDate(date);
        }
        var ary = date.toArray();

        if (ary[2] >= 21) {
            ary[1] = ary[1] + 1;
        }
        var bcDate = new Date(ary[0], ary[1] - 1, 1);
        var bcId = bcDate.Format('yyMM');

        var bcSDate = bcDate.DateAdd('m', -1).Format('YYYY-MM') + '-21';
        var bcEDate = bcDate.Format('YYYY-MM') + '-21';

        return {
            id: bcId,
            startDate: bcSDate,
            endDate: bcEDate
        }

    };

    var pastBCs = (function () {

    })();


    var curBC = (function () {
        var serDate = new Date();
        var bc = BillingCycle(serDate);
        return bc;
    })();

    var furtherBCs = (function () {

    })();

    var getBCByRef = function (bcRef) {
        if (!bcRef || bcRef == 'current')
            return curBC;

        var year = 2000 + parseInt(bcRef.substr(0, 2));
        var month = parseInt(bcRef.substr(2));

        var date = new Date(year, month - 1, 1);
        var bc = BillingCycle(date);
        return bc;
    };

    return {
        pastBCs: pastBCs,
        curBC: curBC,
        furtherBCs: furtherBCs,
        getBCByRef: getBCByRef
    };
});