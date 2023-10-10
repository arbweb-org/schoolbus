var r_map;
var r_mrk;
var r_lat;
var r_lng;

function f_load_map(p_lat, p_lng) {
    r_map = L.map('map').setView([p_lat, p_lng], 13);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(r_map);

    var pinIcon = L.icon({
        iconUrl: 'img/pin.svg',
        iconSize: [60, 60],
        iconAnchor: [30, 30],
        popupAnchor: [0, 0]
    });

    r_lat = p_lat;
    r_lng = p_lng;

    r_mrk = L.marker([p_lat, p_lng], { icon: pinIcon }).addTo(r_map);
}

function f_move_marker(p_lat, p_lng) {
    r_lat = p_lat;
    r_lng = p_lng;

    try {
        r_mrk.slideTo([p_lat, p_lng], {
            duration: 3000,
            keepAtCenter: false
        });
    }
    catch (p_exp) { }
}

function f_center_map() {
    try {
        var latLon = L.latLng(r_lat, r_lng);
        var bounds = latLon.toBounds(5000);
        r_map.panTo(latLon).fitBounds(bounds);
    }
    catch (p_exp) { }
}