(function () {
    var map;
    var marker;
    $(document).ready(function () {
        map = geografija.map.create.CreateMapD("map");
        var displayOpts = geografija.util.pullLocationDisplayOpts();
        marker = geografija.util.setMap(map, displayOpts);
    });
})();