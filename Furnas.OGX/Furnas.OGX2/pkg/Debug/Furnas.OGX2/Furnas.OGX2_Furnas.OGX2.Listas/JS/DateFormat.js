function dateConvert(date) {
    if (date === null) { return null; }
    if (typeof (date) === 'undefined') { return null; }
    var data = new Date(new Date(parseInt(date.substr(6))))

    return data;
}


function dateToString(d) {

    if (d === null) { return ""; }
    var dd = d.getUTCDate();
    var mm = d.getUTCMonth() + 1; //January is 0!
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    return [dd, mm, d.getUTCFullYear()].join('/');
}
