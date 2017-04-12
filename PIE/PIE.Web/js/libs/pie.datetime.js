Date.prototype.isLeapYear = function () {
    return (0 == this.getYear() % 4 && ((this.getYear() % 100 != 0) || (this.getYear() % 400 == 0)));
}

//————————————————— 
// YYYY/yyyy/YY/yy Year 
// MM/M Month
// W/w Week 
// dd/DD/d/D Day 
// hh/HH/h/H Hour 
// mm/m Minutes 
// ss/SS/s/S Seconds 
//————————————————— 
Date.prototype.Format = function (formatStr) {
    var str = formatStr;
    var Week = ['日', '一', '二', '三', '四', '五', '六'];

    str = str.replace(/yyyy|YYYY/, this.getFullYear());
    str = str.replace(/yy|YY/, (this.getYear() % 100) > 9 ? (this.getYear() % 100).toString() : '0' + (this.getYear() % 100));

    str = str.replace(/MM/, this.getMonth() + 1 > 9 ? (this.getMonth() + 1).toString() : '0' + (this.getMonth() + 1));
    str = str.replace(/M/g, this.getMonth() + 1);

    str = str.replace(/w|W/g, Week[this.getDay()]);

    str = str.replace(/dd|DD/, this.getDate() > 9 ? this.getDate().toString() : '0' + this.getDate());
    str = str.replace(/d|D/g, this.getDate());

    str = str.replace(/hh|HH/, this.getHours() > 9 ? this.getHours().toString() : '0' + this.getHours());
    str = str.replace(/h|H/g, this.getHours());
    str = str.replace(/mm/, this.getMinutes() > 9 ? this.getMinutes().toString() : '0' + this.getMinutes());
    str = str.replace(/m/g, this.getMinutes());

    str = str.replace(/ss|SS/, this.getSeconds() > 9 ? this.getSeconds().toString() : '0' + this.getSeconds());
    str = str.replace(/s|S/g, this.getSeconds());

    return str;
}


Date.prototype.DateAdd = function (strInterval, Number) {
    var dtTmp = this;
    switch (strInterval) {
        case 's': return new Date(dtTmp.getTime() + (1000 * Number));
        case 'n': return new Date(dtTmp.getTime() + (60000 * Number));
        case 'h': return new Date(dtTmp.getTime() + (3600000 * Number));
        case 'd': return new Date(dtTmp.getTime() + (86400000 * Number));
        case 'w': return new Date(dtTmp.getTime() + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}


Date.prototype.DateDiff = function (strInterval, dtEnd) {
    var dtStart = this;

    if (typeof dtEnd == 'string') {
        dtEnd = StringToDate(dtEnd);
    }

    switch (strInterval) {
        case 's': return parseInt((dtEnd - dtStart) / 1000);
        case 'n': return parseInt((dtEnd - dtStart) / 60000);
        case 'h': return parseInt((dtEnd - dtStart) / 3600000);
        case 'd': return parseInt((dtEnd - dtStart) / 86400000);
        case 'w': return parseInt((dtEnd - dtStart) / (86400000 * 7));
        case 'm': return (dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - dtStart.getFullYear()) * 12) - (dtStart.getMonth() + 1);
        case 'y': return dtEnd.getFullYear() - dtStart.getFullYear();
    }
}

Date.prototype.toArray = function () {
    var myDate = this;
    var myArray = Array();

    myArray[0] = myDate.getFullYear();
    myArray[1] = myDate.getMonth() + 1;
    myArray[2] = myDate.getDate();
    myArray[3] = myDate.getHours();
    myArray[4] = myDate.getMinutes();
    myArray[5] = myDate.getSeconds();

    return myArray;
}

//+————————————————— 
//| interval can be: 
//| y Year m Month d Day w Week ww Week h Hour n Minutes s Seconds
//+————————————————— 
Date.prototype.DatePart = function (interval) {
    var myDate = this;
    var partStr = '';
    var Week = ['日', '一', '二', '三', '四', '五', '六'];
    switch (interval) {
        case 'y': partStr = myDate.getFullYear(); break;
        case 'm': partStr = myDate.getMonth() + 1; break;
        case 'd': partStr = myDate.getDate(); break;
        case 'w': partStr = Week[myDate.getDay()]; break;
        case 'ww': partStr = myDate.WeekNumOfYear(); break;
        case 'h': partStr = myDate.getHours(); break;
        case 'n': partStr = myDate.getMinutes(); break;
        case 's': partStr = myDate.getSeconds(); break;
    }

    return partStr;
}

Date.prototype.MaxDayOfDate = function () {
    var myDate = this;
    var ary = myDate.toArray();
    var date1 = (new Date(ary[0], ary[1], 1));
    var date2 = date1.DateAdd('m', 1);
    //var result = dateDiff(date1.Format('yyyy-MM-dd'), date2.Format('yyyy-MM-dd')); 
    return date2.DateDiff('d', date1);
}

Date.prototype.WeekNumOfYear = function () {
    var a = new Date(this.getFullYear(), this.getMonth() + 1, this.getDate());
    var b = new Date(this.getFullYear(), 0, 1);
    var d = (a.getTime() - b.getTime()) / 86400000;
    var w = Math.floor(d / 7) + 1;
    if (7 - b.getDay() < d % 7) {
        w++;
    }

    return w;
}

//+————————————————— 
//|The format of DateOne & DateTwo is YYYY-MM-dd 
//+————————————————— 
function daysBetween(DateOne, DateTwo) { 
    var OneMonth = DateOne.substring(5, DateOne.lastIndexOf('-'));
    var OneDay = DateOne.substring(DateOne.length, DateOne.lastIndexOf('-') + 1);
    var OneYear = DateOne.substring(0, DateOne.indexOf('-'));

    var TwoMonth = DateTwo.substring(5, DateTwo.lastIndexOf('-'));
    var TwoDay = DateTwo.substring(DateTwo.length, DateTwo.lastIndexOf('-') + 1);
    var TwoYear = DateTwo.substring(0, DateTwo.indexOf('-'));

    var cha = ((Date.parse(OneMonth + '/' + OneDay + '/' + OneYear) - Date.parse(TwoMonth + '/' + TwoDay + '/' + TwoYear)) / 86400000);
    return Math.abs(cha);
}

//+————————————————— 
//| DatStr format：YYYY-MM-DD or YYYY/MM/DD 
//+————————————————— 
function IsValidDate(DateStr) {
    var sDate = DateStr.replace(/(^\s+|\s+$)/g, ''); //remove empty in string; 
    if (sDate == '') return true;

    //If format is YYYY-(/)MM-(/)DD or YYYY-(/)M-(/)DD or YYYY-(/)M-(/)D or YYYY-(/)MM-(/)D, it will replace to ''.
    var s = sDate.replace(/[\d]{ 4,4 }[\-\/]{ 1 }[\d]{ 1,2 }[\-\/]{ 1 }[\d]{ 1,2 }/g, '');

    //the date has been formated as: YYYY-MM-DD or YYYY-M-DD or YYYY-M-D or YYYY-MM-D 
    if (s != '') {
        var t = new Date(sDate.replace(/\-/g, '/'));
        var ar = sDate.split(/[\-\/:]/);
        if (ar[0] != t.getFullYear() || ar[1] != t.getMonth() + 1 || ar[2] != t.getDate()) {
            return false;
        }
    } else {
        return false;
    }

    return true;
}

//+—————————————————  
//| str format：YYYY-MM-DD HH:MM:SS 
//+————————————————— 
function CheckDateTime(str) {
    var reg = /^(\d+)-(\d{ 1,2 })-(\d{ 1,2 }) (\d{ 1,2 }):(\d{ 1,2 }):(\d{ 1,2 })$/;
    var r = str.match(reg);
    if (r == null) return false;

    r[2] = r[2] - 1;
    var d = new Date(r[1], r[2], r[3], r[4], r[5], r[6]);
    if (d.getFullYear() != r[1]) return false;
    if (d.getMonth() != r[2]) return false;
    if (d.getDate() != r[3]) return false;
    if (d.getHours() != r[4]) return false;
    if (d.getMinutes() != r[5]) return false;
    if (d.getSeconds() != r[6]) return false;

    return true;
}

//+————————————————— 
//| Format: MM/dd/YYYY MM-dd-YYYY YYYY/MM/dd YYYY-MM-dd 
//+————————————————— 
function StringToDate(DateStr) {
    var converted = Date.parse(DateStr);
    var myDate = new Date(converted);
    if (isNaN(myDate)) {
        //var delimCahar = DateStr.indexOf(‘/’)!=-1?’/':’-'; 
        var arys = DateStr.split('-');
        myDate = new Date(arys[0], arys[1] * -1, arys[2]);
    }
    return myDate;
}