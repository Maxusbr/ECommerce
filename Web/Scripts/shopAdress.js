ymaps.ready(init);
function init() {
    var myMap;
    //myPlacemark,
    // Создадим экземпляр элемента управления «поиск по карте»
    // с установленной опцией провайдера данных для поиска по организациям.
    var searchControl = new ymaps.control.SearchControl({
        options: {
            provider: 'yandex#search'
        }
    });
    // Определяем адрес по координатам (обратное геокодирование)
    function getAddress(coords) {
        //myPlacemark.properties.set('iconContent', 'поиск...');
        ymaps.geocode(coords).then(function (res) {
            var firstGeoObject = res.geoObjects.get(0);
            if (firstGeoObject === null) {
                var _city = ''; // если не найден - возвращаем пустую строку
            } else {
                // получаем название города
                var _city = firstGeoObject.properties.get('metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.Locality.LocalityName');
                var _street = firstGeoObject.properties.get('metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.Locality.Thoroughfare.ThoroughfareName');
                var _house = firstGeoObject.properties.get('metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.Locality.Thoroughfare.Premise.PremiseNumber');
                $('.adressSity').val(_city);
                $('.adressStreet').val(_street);
                $('.adressHouse').val(_house);
                changeAdress();
                //myPlacemark.properties
                //.set({
                //    iconContent: firstGeoObject.properties.get('name'),
                //    balloonContent: firstGeoObject.properties.get('text')
                //});
            }

        });
    }
    function createMap() {
        myMap = new ymaps.Map('map', {
            center: [50.4481, 30.5254],
            zoom: 11,
            type: 'yandex#map',
            controls: []
        });
        myMap.events.add('click', function (e) {
            var coords = e.get('coords');
            getAddress(coords);
        });
        myMap.controls.add(searchControl);
    }

    function changeAdress() {
        if (!myMap) createMap();
        var res = $(".adressSity")[0].value;
        if ($(".adressStreet")[0].value) res += ", " + $(".adressStreet")[0].value;
        if ($(".adressHouse")[0].value) res += ", " + $(".adressHouse")[0].value;
        searchControl.search(res);
    }

    $('#hideMap').bind({
        click: function () {
            if (myMap) {
                myMap.destroy();// Деструктор карты
                myMap = null;
            }
            $("#textAdress").show();
            $("#createAdress").hide();
        }
    });

    $('#showMap').bind({
        click: function () {
            if (!myMap) {
                createMap();
                changeAdress();
            }
            $("#textAdress").hide();
            $("#createAdress").show();
        }
    });


    
    // Создание метки
    //function createPlacemark(coords) {
    //    return new ymaps.Placemark(coords, {
    //        iconContent: 'поиск...'
    //    }, {
    //        preset: 'islands#violetStretchyIcon',
    //        draggable: true
    //    });
    //}


    
    if ($('#showMap')[0].innerHTML == "showMap") {
        createMap();
        changeAdress();
    }
    $(".adressInput").change(changeAdress);
}







