function setIconWeather(code, isMorning) {
    var nightCode = isMorning ? "" : "-N";
    // Orage
    if (code >= 200 && code <= 232) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-07_thunderstorm' + nightCode);
        });
    }
    // Nuage + pluies
    if ((code >= 300 && code <= 321) || (code >= 520 && code <= 531)) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-05_showerRain' + nightCode);
        });
    }
    // soleil + pluies
    if (code >= 500 && code <= 504) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-06_rain' + nightCode);
        });
    }
    //Neiges
    if (code >= 600 && code <= 622) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-08_snow' + nightCode);
        });
    }
    //Bruumes
    if (code >= 701 && code <= 781) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-09_mist' + nightCode);
        });
    }
    // Ciel d�gag�
    if (code == 800) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-01_skyIsClear' + nightCode);
        });
    }
    // Soleil + nuages
    if (code == 801) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-02_fewClouds' + nightCode);
        });
    }
    // Nuage 1
    if (code == 802 || code == 803) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-03_scatteredClouds' + nightCode);
        });
    }
    // Nuage 2
    if (code == 804) {
        $('.tab_contentWeather').children('.weather-ico').each(function () {
            $(this).removeAttr('class');
            $(this).addClass('weather-ico');
            $(this).addClass('sprite-04_brokenClouds' + nightCode);
        });
    }
}
function SetWeatherInfo(data) {
    var dt = new Date();
    var today = dt.getDate() + "/" + ("0" + (dt.getMonth() + 1)).slice(-2) + "/" + dt.getFullYear();
    $('.weatherDate').html(today);
    var temp = data.main.temp;
    var temp_max = data.main.temp_max;
    var temp_min = data.main.temp_min;
    $('.weather-infosRight').html(temp);
    $('.weatherInfo').html(data.weather[0].description);
    $('.weatherCity').html(data.name);
    $('.degrees').html(temp + '°C');
    setIconWeather(data.weather[0].id, data.weather[0].icon.indexOf('d') > -1);
}

function GetWeatherInfo(city) {
    var directUrl = "http://api.openweathermap.org/data/2.5/weather";
    var api = '2d3f3c632a918e9e16908ce030dcea6a';
    var param = { q: city, units: "metric", APPID: api };
    try {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: directUrl,
            data : param,
            cache: false,
            success: function (data) {
                SetWeatherInfo(data);
            },
            error: function (errorData) {
                $('.weather-infosLeft').hide();
                $('.weather-infosGlobal').html(errorData.status);
            }
        });
    }
    catch (err) {
        $('.weather-infosLeft').hide();
        $('.weather-infosGlobal').html("Erreur lors de la recherche m�t�o");
    }
}

function setCitiesTabToAutoComplete(cities) {
    var rows = cities.split('\n');
    var citiesTab = new Array(rows.length - 1);
    for (i = 0; i < rows.length - 1; i++) {
        var row = rows[i + 1];
        citiesTab[i] = row.split('\t')[1] + ", " + row.split('\t')[4];
    }

    $("#cities").autocomplete({
        source:function(request, response) {
            var results = $.ui.autocomplete.filter(citiesTab, request.term);
            response(results.slice(0, 5));
        },
        focus: function (event, ui) {
            GetWeatherInfo(ui.item.label);
            return false;
        },
        messages: {
            noResults: '',
            results: function () { }
        }
    });
}

function getAllCities() {
    var url = "/Scripts/city_list.txt";
    $.ajax({
        type: "GET",
        url: url,
        dataType: "text",
        success: function (data) {
            setCitiesTabToAutoComplete(data);
        },
        error: function (err) {
            console.log(JSON.stringify(err));
        }
    });
}

$(document).ready(function () {
    GetWeatherInfo("London,uk");
    getAllCities();
        //.autocomplete("instance")._renderItem = function (ul, item) {
        //return $( "<li>" )
        //  .append( "<a>" + item.label + "</a>" )
        //  .appendTo( ul );
        //}
});