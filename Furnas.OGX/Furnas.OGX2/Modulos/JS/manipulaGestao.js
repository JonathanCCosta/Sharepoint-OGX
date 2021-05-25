function updateAddNewLinks() {
    $("a[id^='idHomePageNewItem']").each(function () {
        var url = location.href;
        //this.href = this.href + "&Source=" + url.split("&",1) + "&RootFolder=" + getQuerystring("ID");//+"&IsDlg=1";
        this.href = this.href + "&Source=" + url.split("&", 1) + "&RootFolder=" + $('h3:contains("Numeração")').closest('td').next('td').text().trim();
        this.onclick = "";
    });
}

function getQuerystring(ji) {
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}
_spBodyOnLoadFunctionNames.push("updateAddNewLinks");