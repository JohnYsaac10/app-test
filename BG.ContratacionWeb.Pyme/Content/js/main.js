'use strict';

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var BgModal = function () {
    function BgModal() {
        _classCallCheck(this, BgModal);

        this._declareTriggers();
    }

    _createClass(BgModal, [{
        key: 'init',
        value: function init() {
            this._declareTriggers();
        }
    }, {
        key: '_declareTriggers',
        value: function _declareTriggers() {
            var _this = this;

            var triggerModal = Array.prototype.slice.call(document.querySelectorAll('[bg-modal]'));
            if (triggerModal.length > 0) {
                (function () {
                    _this._createOverlay();
                    var global = _this;

                    triggerModal.forEach(function (trigger) {
                        trigger.addEventListener('click', function (e) {
                            e.preventDefault();
                            var id = this.getAttribute('bg-modal');
                            var modalId = id;


                            var modal = document.getElementById(modalId);
                            var overlay = document.querySelector('.bg-overlay');
                            if (modal.classList.contains('bg-show')) {
                                global._hideModal(modal, overlay);
                            } else {
                                global._showModal(modal, overlay);
                            }
                        });
                    });
                })();
            }
        }
    }, {
        key: '_createOverlay',
        value: function _createOverlay() {
            var overlay = document.createElement('div');
            overlay.classList.add('bg-overlay');
            document.body.appendChild(overlay);
            return overlay;
        }
    }, {
        key: '_hideModal',
        value: function _hideModal(modal, overlay) {
            modal.classList.remove('bg-show');
            modal.classList.add('bg-close');
            overlay.classList.remove('bg-show');
        }
    }, {
        key: '_showModal',
        value: function _showModal(modal, overlay) {
            modal.classList.remove('bg-close');
            modal.classList.add('bg-show');
            overlay.classList.add('bg-show');
            overlay.addEventListener('click', function (e) {
                if (!modal.dataset.static) {
                    overlay.classList.remove('bg-show');
                    modal.classList.remove('bg-show');
                }
            });
        }
    }, {
        key: '_getOverlay',
        value: function _getOverlay() {
            var overlay = document.querySelector('.bg-overlay');
            if (overlay) {
                return overlay;
            } else {
                return this._createOverlay();
            }
        }
    }, {
        key: 'showModal',
        value: function showModal(modalId) {
            var modal = document.getElementById(modalId);
            if (!modal) {
                throw new Error('No hay Modal para abrir!');
            }
            var overlay = this._getOverlay();
            this._showModal(modal, overlay);
        }
    }, {
        key: 'hideModal',
        value: function hideModal(modalId) {
            var modal = document.getElementById(modalId);
            if (!modal) {
                throw new Error('No hay Modal para remover!');
            }
            var overlay = document.querySelector('.bg-overlay');
            this._hideModal(modal, overlay);
        }
    }]);

    return BgModal;
}();


function validaCedula(cedula) {
    try {
        var i = 0;
        var Verificador = 0;
        var coefValCedula = [2, 1, 2, 1, 2, 1, 2, 1, 2];
        var cedulaCorrecta;

        if (isNaN(cedula)) {
            cedulaCorrecta = false;
        }
        else {
            if (cedula.substr(0, 3) == "000") {
                cedulaCorrecta = false;
            }
            else {

                if (cedula.length == 10) {
                    // PROVINCIA  DE EMISION DE RUC
                    // ECUADOR: 1-24 
                    // EXTRANJEROS: 30
                    var provincia = cedula.substr(0, 2);
                    if ((provincia > 0 && provincia <= 24) || provincia == 30) {

                        var tercerDigito = parseInt(cedula.substr(2, 1));
                        if (tercerDigito < 7) {
                            // Coeficientes de validación cédula
                            // El decimo digito se lo considera dígito verificador
                            var verificador = parseInt(cedula.substr(9, 1));// obtiene EL 10
                            var suma = 0;
                            var digito = 0;
                            for (var i = 0; i < (cedula.length - 1) ; i++) {
                                digito = parseInt(cedula.substr(i, 1)) * coefValCedula[i];
                                suma += parseInt(((digito % 10)) + parseInt((digito / 10)));
                            }

                            if ((suma % 10 == 0) && (suma % 10 == verificador)) {
                                cedulaCorrecta = true;
                            } else if ((10 - (suma % 10)) == verificador) {
                                cedulaCorrecta = true;
                            } else {
                                cedulaCorrecta = false;
                            }
                        } else {
                            cedulaCorrecta = false;// valida tercer digito
                        }
                    } else {
                        cedulaCorrecta = false;
                    }
                } else {
                    cedulaCorrecta = false;

                }
            }
        }

        return cedulaCorrecta;
    }
    catch (err) {
        return false;
    }
}

function formatMoney(number, decPlaces, decSep, thouSep) {
    decPlaces = isNaN(decPlaces = Math.abs(decPlaces)) ? 2 : decPlaces,
    decSep = typeof decSep === "undefined" ? "." : decSep;
    thouSep = typeof thouSep === "undefined" ? "," : thouSep;
    var sign = number < 0 ? "-" : "";
    var i = String(parseInt(number = Math.abs(Number(number) || 0).toFixed(decPlaces)));
    var j = (j = i.length) > 3 ? j % 3 : 0;

    return sign +
        (j ? i.substr(0, j) + thouSep : "") +
        i.substr(j).replace(/(\decSep{3})(?=\decSep)/g, "$1" + thouSep) +
        (decPlaces ? decSep + Math.abs(number - i).toFixed(decPlaces).slice(2) : "");
}


function GeneralPost(postTo, objToSend, CBfuncion) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $(".bg-primary").addClass("bg-loader");
    $.ajax({
        type: "POST",
        traditional: true,
        cache: false,
        url: postTo,
        context: document.body,
        headers: { "__RequestVerificationToken" : token },
        data: objToSend,
        success: function (result) {
            switch (result.codError) {
                case '200':
            CBfuncion(result);
                    break;
                case '400':
                    alertify.error(result.mensaje);
                    break;
                case '401':
                    UnAuthorize();
                    break;
                case '503':
                    alertify.error(result.mensaje);
                    break;
                case '405':
                    location.reload();
                    break;
            }
            $(".bg-primary").removeClass("bg-loader");
        },
        error: function (xhr) {
            alertify.error("Upps! algo salió mal. servicio no disponible. intente mas tarde");
            $(".bg-primary").removeClass("bg-loader");
        }
    });
}

function CustomPost(postTo, objToSend, CBfuncion) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        dataType: 'json',
        cache: false,
        url: postTo,
        headers: { "__RequestVerificationToken": token },
        context: document.body,
        data: objToSend,
        success: function (result) {
            CBfuncion(result);
        },
        error: function (xhr) {
            alertify.error("Upps! algo salió mal. servicio no disponible. intente mas tarde");
        }
    });
}


function createObjectForSolicitud() {

        var radioValue = $("input[name='tipoSobregiro']:checked").val();
        var radios = document.getElementsByName("tipoSobregiro");
        var cta= $("#select_cuenta_id").children("option:selected").val();
        var monto = $('#cantidad_solicitada_id').val().replace('$', '');
        var fecha = $('#fcplazo').val();
        var formValid = false;

        var rango = $("#rangoId").val().split("-");

        var i = 0;
        while (!formValid && i < radios.length) {
            if (radios[i].checked) formValid = true;
            i++;        
        }

        
        if (cta === "0") {
            alertify.error("Debes elegir una cuenta para el sobregiro");
            return false;
        } 
        if (monto === "0" || monto === undefined || monto === null || monto === "") {
            alertify.error("Debes colocar un monto a financiar");
            return false;
        }
        if (+monto < rango[0]) {
            alertify.error("Debes colocar un monto mayor a " + rango[0]);
            return false;
        }
        if (+monto > rango[1]) {
            alertify.error("Debes colocar un monto menor a " + rango[1]);
            return false;
        }
        

        if (fecha === undefined || fecha === null || fecha === "") {
            alertify.error("Debes colocar una fecha");
            return false;
        } 
        if (!formValid) {
            alertify.error("Debes elegir un tipo de sobregiro");
            return formValid;
        } 

        var obj = {
            "cuenta": cta,
            "cantidad": monto,
            "fechaPago": fecha,
            "tipoSobregiro": radioValue
        };
        return obj;
}

function UnAuthorize() {
    var urlHome = $('#UrlNotificacion').val();
    alertify.error("Su sesion ha expirado");
    setTimeout(function () {
        window.location.href = urlHome + '/401';
    }, 2000);
}

function ReenviarOTP() {
   
    bgmodal.hideModal('myIdOtp');
    var text = $('#titleModalOTP_id').text();
    $('#titleModalOTP_id').text(text.replace('Enviamos', 'Reenviamos'));
    SolicitarOTP();
    blincktext();
}

function blincktext() {
    var f = document.getElementById('titleModalOTP_id');
    var myVar;
    myVar = setInterval(function () {
        f.style.visibility = (f.style.visibility == 'hidden' ? '' : 'hidden');
    }, 500);

    setTimeout(function () {
        clearInterval(myVar);
        f.style.visibility = '';
    }, 2500);
}



function OnChangeModoPago() {
    var selectedTextForma = $("#idModoPago").find("option:selected").text();
    var SelectedTextDest = $("#idDestinoCredito").find("option:selected").text();

    $('#idPlazoCredito').prop('selectedIndex', 0);

    if (SelectedTextDest === "Capital de Trabajo" && selectedTextForma === "Mensualmente") {
        $('option:contains("15")').hide();
        $('option:contains("18")').hide();
        $('option:contains("21")').hide();
    }

    if (SelectedTextDest != "Capital de Trabajo" && selectedTextForma === "Mensualmente") {
        $('option:contains("15")').show();
        $('option:contains("18")').show();
        $('option:contains("21")').show();

    }

    if (selectedTextForma === "Al Vencimiento") {
        $('option:contains("15")').show();
        $('option:contains("18")').show();
        $('option:contains("21")').show();
        $("#idBoxCalendarioHide").show();
        $("#idTitleWheneAlVenc").show();
        $("#idTitleWhenMen").hide();
        $("#idDiaPagoNumero").val('');
        $('#idLinkTablaAmort').hide();
        $("#idBoxDiaHide").hide();
        $("#idInabilitarBoxCalPla").hide();
    } else {
        $("#idTitleWheneAlVenc").hide();
        $("#idTitleWhenMen").show();
        $("#idBoxDiaHide").show();
        $("#idBoxCalendarioHide").hide();
        $("#idDiaPago").val('');
        $("#idInabilitarBoxCalPla").show();
        $('#idLinkTablaAmort').show();
    }
}

function OnChangeDestinoCredito() {
    var SelectedTextForma = $("#idModoPago").find("option:selected").text();
    var SelectedTextDest = $("#idDestinoCredito").find("option:selected").text();

    $('#idPlazoCredito').prop('selectedIndex', 0);

    if (SelectedTextDest != "Capital de Trabajo") {
        $('option:contains("15")').show();
        $('option:contains("18")').show();
        $('option:contains("21")').show();
    }

    if (SelectedTextForma === "Mensualmente" && SelectedTextDest === "Capital de Trabajo") {
        $('option:contains("15")').hide();
        $('option:contains("18")').hide();
        $('option:contains("21")').hide();
    }
}


function SolicitarOTP() {

    var urlPostSolicitar = $('#UrlSolicitarOTP').val();
    GeneralPost(urlPostSolicitar, { 'data': 'Sobregiro' }, function (resp) {
            bgmodal.showModal('myIdOtp');
    });
}


function ValidarOTP() {
    
    $('#firmar_otp_id').addClass("bg-loader");

    var urlPostSolicitar = $('#UrlValidarOTP').val();
    var code = $('#code_opt_id').val();
    var newPage = $("#UrlNotificacion").val();

    if (code != "" && code.length === 6) {
        GeneralPost(urlPostSolicitar, { 'data': code }, function (resp) {

            window.location.href = newPage +'/' + resp.redirectCode;

            $('#firmar_otp_id').removeClass("bg-loader");
        });
    } else {
        alertify.error("Debe ingresar todos los números");
        $('#firmar_otp_id').removeClass("bg-loader");
    }
}

////////////------------  funciones para credito pyme -----------

function CalcularCreditoPyme() {
    var url = $("#UrlCalcularPost").val();
    console.log(url);
    var obj = createObjForSolicitudCP();
    console.log(obj);
    if (obj) {
        GeneralPost(url, obj, function (resp) {
            console.log(resp)
                $("#idCuotaCalculada").text(formatMoney(resp.cuota));
                bgmodal.showModal('cuotaMensual');
        });
    }
}


function createObjForSolicitudCP() {   //ñññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññ

    var token = $('input[name="__RequestVerificationToken"]').val();
    var rangeMonto = $("#idRangeMonto").val().split('-');
    var rangeDays = $("#idRangeCalendarDays").val().split('-');
    var monto = $('#cantidad_solicitada_id').val().replace('$', '').replace(',','');
    var destinoCreditoId = $("#idDestinoCredito").children("option:selected").val();
    var destinoCredito = $("#idDestinoCredito").children("option:selected").text();
    var modoPagoId = $("#idModoPago").children("option:selected").val(); 
    var modoPago = $("#idModoPago").children("option:selected").text(); 
    var plazoCredito = $("#idPlazoCredito").children("option:selected").text();
    var plazoCreditoId = $("#idPlazoCredito").children("option:selected").val();
    var tipoAmortizacion = $("input[name='amortizacion']:checked").val();
    var diaPago; var fechaPago;

    if (monto === "0" || monto === undefined || monto === null || monto === "") {
        alertify.error("Debe colocar un monto");
        return false;
    }
    if (+monto < +rangeMonto[0] || +monto > +rangeMonto[1]) {
        alertify.error("Debe colocar un monto de " + formatMoney(rangeMonto[0]) + " hasta " + formatMoney(rangeMonto[1]));
        return false;
    }
    if (modoPagoId === "0" || modoPagoId === undefined || modoPagoId === null || modoPagoId === "") {
        alertify.error("Debe seleccionar como va a pagar su crédito");
        return false;
    }

    if (modoPago === "Al Vencimiento") {
        diaPago = 0;
        fechaPago = $("#idDiaPago").val();

        if (fechaPago === "0" || fechaPago === undefined || fechaPago === null || fechaPago === "") {
            alertify.error("Debes ingresar dia de pago");
            return false;
    }


        var parts = fechaPago.split('/');
        if (parts.length != 3) {
            alertify.error("Formato de fecha incorrecto");
        return false;
    }
        var maxdaysR = rangeDays[0] + rangeDays[1];
        var mydate = new Date(parts[2], parts[1] - 1, parts[0]);
        var dateMin1 = new Date();
        dateMin1.setDate(dateMin1.getDate() + rangeDays[0]);
        var dateMax1 = new Date();
        dateMax1.setDate(dateMax1.getDate() + maxdaysR);
        if (mydate < dateMin || mydate > dateMax1) {
            alertify.error("Fecha no permitida, no se encuentra dento del rango");
        return false;
        }
    }

    if (modoPago === "Mensualmente") {
        
        fechaPago = "";
        diaPago = $("#idDiaPagoNumero").val();

        if (diaPago === "0" || diaPago === undefined || diaPago === null || diaPago === "") {
            alertify.error("Ingrese el dia de pago");
        return false;
    }
        if (diaPago > 30 || diaPago < 1) {
            alertify.error("Rango de dia no permitido");
        return false;
        }
    }

    
    if (destinoCreditoId === "0" || destinoCreditoId === undefined || destinoCreditoId === null || destinoCreditoId === "") {
        alertify.error("Debe seleccionar un destino de credito");
        return false;
    }


    var obj = {
        "Monto": monto,
        "DestinoCredito": { "idCodigo": destinoCreditoId, "Descripcion": destinoCredito },
        "ModoDePago": { "idCodigo": modoPagoId, "Descripcion": modoPago },
        "PlazoCredito": { "idCodigo": plazoCreditoId, "Descripcion": plazoCredito },
        "Amortizacion": tipoAmortizacion,
        "DiaDePago": diaPago,
        "fechaPago": fechaPago
    };

    return { "dataJson": JSON.stringify(obj) };
}

/*
function SendEstadoCivil() {
    var estado = $("#idSelectEstado").children("option:selected").val();
    if (estado === "0" || estado === undefined || estado === null || estado === "") {
        alertify.alert("Debe seleccionar su estado civil");
    }else{
        var urlPost = $("#IdUrlPostEstado").val();
        var urlContinue = $("#IdUrlBtnContinueToDom").val();
        var urlNoti = $("#UrlNotificacion").val();
        
        GeneralPost(urlPost, {"data" : estado}, function (resp) {
            if(resp.errorCode == "200")
                window.location.href = urlContinue;
            if(resp.errorCode == "400")
                alertify.alert("Debe seleccionar el estado civil");
            if (resp.errorCode == "401")
                window.location.href = urlNoti + '/401';
        });
    }
    
}*/


/* ññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññ
function SendDirDomicilio() {
    var telefono = $("#idTelefonoDom").val();
    var domicilio = $("#idInputDom").val();

    if ($("#idCampoDomHide").is(":hidden")) {
        domicilio = $("#idParrafoDir").text().trim();
    }
    console.log(domicilio);

    if (telefono && domicilio) {
        var url = $("#urlPostInfoDom").val();
        var obj = {};
        obj["Telefono"] = telefono;
        obj["Direccion"] = domicilio;
        GeneralPost(url, obj, function (resp) {
            if (resp.codError === "200") {
                window.location.href = $("#urlToResumenInfoPers").val();
            } else {
                alertify.error(resp.mensaje);
            }
        });
    }
    else {
        alertify.error("Todos los campos son requeridos");
    }
}      */  //ñññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññ

/*function SendBalances() {       //ññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññññ
    var file1 = document.getElementById("documento1").files[0];
    var file2 = document.getElementById("documento2").files[0];
    var file3 = document.getElementById("documento3").files[0];
    var bandera = true;
    
    if (typeof file1 === 'undefined' || typeof file2 === 'undefined' || typeof file3 === 'undefined') {
        alertify.error('faltan archivos por subir'); bandera = false;
    }
    else {
        if (file1.type != 'application/pdf' || file2.type != 'application/pdf' || file3.type != 'application/pdf') {
            alertify.error('solo se admiten formatos con extención .pdf'); bandera = false;
        }
    }

    if (bandera) {
        obj = { "file1": file1, "file2": file2, "file3": file3 };
        GeneralPost("", obj, function () { })
    }
}   */

function SendVentas() {
   
    var obj = {};
    var arr = [];
    var url = $("#urlPostVentas").val();
    var allGood = true;
    var arrayNombreProductos = $("input[name='nombreProducto[]']")
              .map(function(){return $(this).val();}).get();
    var arrayPorcentajeProductos = $("input[name='porcentajeProducto[]']")
              .map(function () { return $(this).val(); }).get();
    var numeroEmpleados = $('#numeroEmpleados').val();
    
    
     if (numeroEmpleados === "0" || numeroEmpleados === undefined || numeroEmpleados === null || numeroEmpleados === "") {
         alertify.error("Debe ingresar número de empleados");
    }
    else {
        for (var i = 0; i < arrayNombreProductos.length; i++) 
        {
            if (arrayNombreProductos[i] === "0" || arrayNombreProductos[i] === undefined || arrayNombreProductos[i] === null || arrayNombreProductos[i] === "") {
                alertify.error("Debe ingresar nombre del producto");
                allGood = false;
                break;
            }
            else if (arrayPorcentajeProductos[i] === "0" || arrayPorcentajeProductos[i] === undefined || arrayPorcentajeProductos[i] === null || arrayPorcentajeProductos[i] === "") {
                alertify.error("Debe ingresar porcentaje de ventas del producto");
                allGood = false;
                break;
            }
            else {
                var Producto = {};
                Producto.nombre = arrayNombreProductos[i];
                Producto.porcentajeVentas = arrayPorcentajeProductos[i];
                arr.push(Producto);
            }
        }
        
        if (allGood == true) {
            $('#idBtnContinueToClientes').addClass("bg-loader");
        CustomPost(url, JSON.stringify({ "productos": arr, "numeroEmpleados": numeroEmpleados }), function (resp) {
            if (resp.codError === "200") {
                window.location.href = $("#urlToClientes").val();
            } else {
                alertify.error(resp.mensaje);
            }
        });
        }

    }
}

function SendClientesProveedores() {
    
    var obj = {};
    var arr = [];
    var allGood = true;
    var url = $("#urlPostClientes").val();
    var arrayNombreClientes = $("input[name='nombreCliente[]']")
              .map(function () { return $(this).val(); }).get();
    var arrayPorcentajeClientes = $("input[name='porcentajeCliente[]']")
              .map(function () { return $(this).val(); }).get();
    var arrayDiasClientes = $("input[name='diasCliente[]']")
              .map(function () { return $(this).val(); }).get();
    var arrayNombreProveedores = $("input[name='nombreProveedor[]']")
              .map(function () { return $(this).val(); }).get();
    var arrayPorcentajeProveedores = $("input[name='porcentajeProveedor[]']")
              .map(function () { return $(this).val(); }).get();
    var arrayDiasProveedores = $("input[name='diasProveedor[]']")
              .map(function () { return $(this).val(); }).get();
    
    
    
    
        for (var i = 0; i < arrayNombreClientes.length; i++) {

            if (arrayNombreClientes[i] === "0" || arrayNombreClientes[i] === undefined || arrayNombreClientes[i] === null || arrayNombreClientes[i] === "") {
                alertify.error("Debe ingresar nombre del cliente");
                allGood = false;
                break;
            }
            else if (arrayPorcentajeClientes[i] === "0" || arrayPorcentajeClientes[i] === undefined || arrayPorcentajeClientes[i] === null || arrayPorcentajeClientes[i] === "") {
                alertify.error("Debe ingresar porcentaje de ventas al cliente");
                allGood = false;
                break;
            }
            else if (arrayDiasClientes[i] === undefined || arrayDiasClientes[i] === null || arrayDiasClientes[i] === "") {
                alertify.alert("Debe ingresar días de plazo al cliente");
                allGood = false;
                break;
            }
            else {
                var ClientesProveedores = {};
                ClientesProveedores.nombre = arrayNombreClientes[i];
                ClientesProveedores.porcentaje = arrayPorcentajeClientes[i];
                ClientesProveedores.dias = arrayDiasClientes[i];
                ClientesProveedores.tipo = 'C';
                arr.push(ClientesProveedores);
            }
        }

        for (var i = 0; i < arrayNombreProveedores.length; i++) {
            if (arrayNombreProveedores[i] === "0" || arrayNombreProveedores[i] === undefined || arrayNombreProveedores[i] === null || arrayNombreProveedores[i] === "") {
                alertify.error("Debe ingresar nombre del proveedor");
                allGood = false;
                break;
            }
            else if (arrayPorcentajeProveedores[i] === "0" || arrayPorcentajeProveedores[i] === undefined || arrayPorcentajeProveedores[i] === null || arrayPorcentajeProveedores[i] === "") {
                alertify.error("Debe ingresar porcentaje de compras al proveedor");
                allGood = false;
                break;
            }
            else if (arrayDiasProveedores[i] === "0" || arrayDiasProveedores[i] === undefined || arrayDiasProveedores[i] === null || arrayDiasProveedores[i] === "") {
                alertify.error("Debe ingresar días de crédito del proveedor");
                allGood = false;
                break;
            }
            else {

                var ClientesProveedores = {};
                ClientesProveedores.nombre = arrayNombreProveedores[i];
                ClientesProveedores.porcentaje = arrayPorcentajeProveedores[i];
                ClientesProveedores.dias = arrayDiasProveedores[i];
                ClientesProveedores.tipo = 'P';
                arr.push(ClientesProveedores);
            }
        }

        if (allGood == true) {
            $('#idBtnContinueToNegocio').addClass("bg-loader");
            CustomPost(url, JSON.stringify({ "clientes": arr }), function (resp) {
                if (resp.codError === "200") {
                    window.location.href = $("#urlToNegocio").val();

                } else {
                    alertify.error(resp.mensaje);
                }
            });
        }
    
}


function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function ValidatePostDomicilio(isNegocio) {
    var IdDireccionEsDe = $("#idSelectDom").find("option:selected").val();
    var direccion1 = $("#idDireccionField1").val();
    var direccion2 = $("#idDireccionField2").val();
    var referencia = $("#idReferenciaField").val();

    var idProvincia = $("#idSeleccionProvincia").find("option:selected").val(); 
    var ProvinciaDesc = $("#idSeleccionProvincia").find("option:selected").text();
    var idCiudad = $("#idSeleccionCiudad").find("option:selected").val();
    var CiudadDesc = $("#idSeleccionCiudad").find("option:selected").text();
    var idParroquia = $("#idSeleccionParroquia").find("option:selected").val();
    var ParroquiaDesc = $("#idSeleccionParroquia").find("option:selected").text();

    var telefonoDomicilio = $("#idTelefonoDom").val();
    var obj = {}

    if (IdDireccionEsDe === 'N' || IdDireccionEsDe === 'T' || isNegocio) {

        if (direccion1 === '') {
            alertify.error('Olvidates ingresar dirección');
            return false;
        }

    }

    if (direccion2 === '') {
        alertify.error('Olvidates ingresar dirección');
            return false;
        }

    if (referencia === '') {
        alertify.error('Olvidates ingresar referencia');
            return false;
        }

    if (idProvincia === '0') {
        alertify.error('olvidates seleccionar provincia');
        return false;
    }

    if (idCiudad === '0') {
        alertify.error('olvidates seleccionar ciudad');
        return false;
    }

    if (idParroquia === '0') {
        alertify.error('olvidates seleccionar parroquia');
        return false;
        }

    if (telefonoDomicilio === '') {
        alertify.error('Falta ingresar el teléfono.');
        return false;
}

    if (telefonoDomicilio.length < 9) {
        alertify.error('Teléfono incompleto.');
        return false;
    }

    if (isNegocio) {
        var isSecure = $("#idSeleccionIsCare").val();
        var aseguradora = $("#idSeleccionAseguradora").val();
        var tipoInmueble = $("#idSeleccionTipoInmueble").val();

        var isSecureDesc = $("#idSeleccionIsCare").text();
        var aseguradoraDesc = $("#idSeleccionAseguradora").text();
        var tipoInmuebleDesc = $("#idSeleccionTipoInmueble").text();
        obj["tipoDomicilio"] = "Negocio";

        obj["isSecure"] = isSecure;
        obj["aseguradora"] = aseguradora;
        obj["tipoInmueble"] = tipoInmueble;
        obj["isSecureDesc"] = isSecureDesc;
        obj["aseguradoraDesc"] = aseguradoraDesc;
        obj["tipoInmuebleDesc"] = tipoInmuebleDesc;
    } else {
        obj["tipoDomicilio"] = IdDireccionEsDe;
    }
    
    obj["direccion1"] = direccion1;
    obj["direccion2"] = direccion2;
    obj["referencia"] = referencia;

    obj["provincia"] = idProvincia;
    obj["ciudad"] = idCiudad;
    obj["parroquia"] = idParroquia;
    obj["provinciaDesc"] = ProvinciaDesc;
    obj["ciudadDesc"] = CiudadDesc;
    obj["parroquiaDesc"] = ParroquiaDesc;

    obj["telefonoDomNeg"] = telefonoDomicilio;

    return obj;
    }

function PostInfoDomAndNeg(isNeg, href) {

    var urlToPost = $("#urlPostInfoDom").val();
    var obj = ValidatePostDomicilio(isNeg);
   
    if (obj) {
        GeneralPost(urlToPost, obj, function (resp) {
                window.location.href = href;
        });
    }
}


function PostSelectedLocation(idSelected, area) {

    var ComboToFill = document.getElementById("idSeleccion" + area);
    var urlToPost = $("#urlToPostArea").val();

    if (idSelected === "0")
        return false;
    
    GeneralPost(urlToPost, { "AreaType": area, "IdSeleccionPadre": idSelected }, function (resp) {

            var len = resp.catalogo.length;

            $(ComboToFill).empty();
            $(ComboToFill).append("<option value='0'>Selecciona la " + area + "</option>");
            for (var i = 0; i < len; i++) {
                var id = resp.catalogo[i]['IdCodigo'];
                var name = resp.catalogo[i]['Descripcion'];

                $(ComboToFill).append("<option value='" + id + "'>" + name + "</option>");

      
            }
    });
}

function PostDocument(e) {
    
    var token = $('input[name="__RequestVerificationToken"]').val();
    var files = e.target.files;
    var doc = e.target.id;
    var urlToPost = $("#urlToPostFiles").val();
    if (files.length > 0) {
        $(".bg-primary").addClass("bg-loader");
        if (window.FormData !== undefined) {
            var data = new FormData();
            var anyoDoc = e.target.getAttribute('name');
            
            for (var x = 0; x < files.length; x++) {
                data.append(doc + '-' + anyoDoc, files[x]);
            }
            $.ajax({
                type: "POST",
                url: urlToPost,
                contentType: false,
                processData: false,
                headers: { "__RequestVerificationToken": token },
                data: data,
                success: function (result) {
                    if (result) {
                        if (result.codError === "200") {
                            alertify.success(result.mensaje);
                            $('#' + anyoDoc).removeClass("fa-upload");
                            $('#' + anyoDoc).addClass("fa-check");
                            $('#btn-' + anyoDoc).addClass("success");
                        }else {
                            alertify.error(result.mensaje);
                            $('#' + anyoDoc).removeClass("fa-check");
                            $('#' + anyoDoc).addClass("fa-upload");
                            $('#btn-' + anyoDoc).removeClass("success");
                        }
                    }
                    $(".bg-primary").removeClass("bg-loader");
                    
                },
                error: function (xhr) {
                    alertify.error("Upps! algo salió mal. servicio no disponible. intente mas tarde");
                    $(".bg-primary").removeClass("bg-loader");
                    $('#' + anyoDoc).removeClass("fa-check");
                    $('#' + anyoDoc).addClass("fa-upload");
                    $('#btn-' + anyoDoc).removeClass("success");
                }
            });
        } else {
            alert("Algo salió mal! por favor intenta de nuevo.");
        }
    }

}

function GenerarVariacionesFE() {

    var urlToRedirect = $("#urlToGenerarIndicadores").val();
    var periodos = [];
    var anio1 = $("#documento1").attr("name");
    var anio2 = $("#documento2").attr("name");
    var anio3 = $("#documento3").attr("name");

    if (anio1) {
        var r = $("#btn-" + anio1).hasClass('success');
        periodos.push(r);
    }
    if (anio2) {
        var w = $("#btn-" + anio2).hasClass('success');
        periodos.push(w);
    }
    if (anio3) {
        var z = $("#btn-" + anio3).hasClass('success');
        periodos.push(z);
    }

    //window.location.href = urlToRedirect;

    var l = periodos.length;
    if (l === 0) {
        window.location.href = urlToRedirect;
    }
    if (l === 1) {
        if (periodos[0] === true) {
            window.location.href = urlToRedirect;
        }
    }
    if (l === 2) {
        if (periodos[0] === true && periodos[1] === true) {
            window.location.href = urlToRedirect;
        }
    }
    if (l === 3) {
        if (periodos[0] === true && periodos[1] === true && periodos[2] === true) {
            window.location.href = urlToRedirect;
        }
    }

    alertify.error("Te faltan subir archivo(s)");
}


function validateCertificaciones() {

    var combo = $('#idSeleccionTieneCert').children("option:selected").val();

    var selected = [];
    $('.col-lg-12 input:checked').each(function () {
        var item = { "idCodigo": $(this).attr('value'), "Descripcion": $(this).attr('name') }
        selected.push(item);
    });

    var nombreCert = $('#idFielDetailCert').val();

    if (combo == 1) {
        if (selected.length < 1) {
            alertify.error('Debes seleccionar al menos una certificación');
            return false;
        }

        var e = selected.find(ele => ele.Descripcion === 'Otra');
        if (e) {
            if (nombreCert === '') {
                alertify.error('Olvidaste Ingresar el nombre de tu certificación');
                return false;
            } else {
                var ref = nombreCert;
                for (var i in selected) {
                    if (selected[i].Descripcion == 'Otra') {
                        selected[i].Descripcion = ref;
                        break;
                    }
                }
            }
        }
    }
    
    var obj = {
        "tieneCertificacion": combo,
        "Certificaciones": selected
    }

    var objRes = { "data": JSON.stringify( obj ) }

    return objRes
}

function SendCertificaciones() {
    var obj = validateCertificaciones();
    if (obj) {
        var url = $('#UrlToPostCert').val();
        var urlToNexPage = $('#UrlToResumenNeg').val();
        GeneralPost(url, obj, function (resp) {
            window.location.href = urlToNexPage;
        })
    }
}

function GetAllFieldsCuentas() {
    var selectedBancos = [];
    var selectedTipoCtas = [];
    var numCtas = [];
    var item = {};
    var arrayObjs = [];
    var enviar = true;
    $('.idBancosSeleccion').each(function () {
        var value = $(this).children("option:selected").val();
        if (value)
            item = { "idBanco": value }
        else {
            alertify.error("olvidaste seleccionar: institución financiera")
            enviar = false;
        }
        selectedBancos.push(item);
    });

    if (enviar === false)
        return enviar;

    $('.idTipoCuentaSeleccion').each(function () {
        var value = $(this).children("option:selected").val();
        if (value)
            item = { "tipo": value }
        else {
            alertify.error("olvidaste seleccionar: tipo cuenta")
            enviar = false;
        }
        selectedTipoCtas.push(item);
    });
    if (enviar === false)
        return enviar;

    $('.idNumeroCuenta').each(function () {
        var text = $(this).val();
        if (text!= "")
            item = { "numeroCuenta": text }
        else {
            alertify.error("olvidaste ingresar: número cuenta")
            enviar = false;
        }
        numCtas.push(item);
    });
    if (enviar === false)
        return enviar;
    for (var i = 0; i < numCtas.length; i++) {
        var obj1 = selectedBancos[i];
        var obj2 = selectedTipoCtas[i];
        var obj3 = numCtas[i];
        var obj = $.extend({}, obj1, obj2, obj3);
        arrayObjs.push(obj);
    }
    return arrayObjs;
}

function CreateCbxModalCta() {
    var arrayObjs = GetAllFieldsCuentas();
    if (arrayObjs) {

        var url = $("#UrlToSendReferenciaBanco").val();
        CustomPost(url, JSON.stringify(arrayObjs), function (resp) {
            if (resp.codError === "404") {
                bgmodal.showModal('abroCuenta');
            }

            if (resp.codError === "200") {
                var array = resp.numCtas;
                $("#idSelectCtaDepModal").empty();
                $("#idSelectCtaDepModal").append("<option value='' disabled selected>Elige tu cuenta</option>");
                $("#idSelectCtaDebModal").empty();
                $("#idSelectCtaDebModal").append("<option value='' disabled selected>Elige tu cuenta</option>");
                
                array.forEach(ele => {
                    $("#idSelectCtaDepModal").append("<option value='" + ele + "'>Cuenta de ahorros " + ele + "</option>");
                    $("#idSelectCtaDebModal").append("<option value='" + ele + "'>Cuenta de ahorros " + ele + "</option>");
                })
                bgmodal.showModal('idSelectCuentaModal');
            }

            if (resp.codError === "503")
                alertify.error(resp.mensaje);
        });
    }
}

function createObjToSendCtas() {
    var obj = {};
    var cuentaDep = $('#idSelectCtaDepModal').children("option:selected").val();
    var cuentaDeb = $('#idSelectCtaDebModal').children("option:selected").val();

    if (cuentaDep) {
        obj["cuentaDep"] = cuentaDep
    } else {
        alertify.error("debe seleccionar la cuenta para realizar el deposito")
        return false;
    }

    if (cuentaDeb) {
        obj["cuentaDeb"] = cuentaDeb
    } else {
        alertify.error("debe seleccionar la cuenta para realizar el debito")
        return false;
    }

    return obj;
}

function SendCtasSelectBg() {
    var obj = createObjToSendCtas();

    if (obj) {
        var url = $("#UrlToSendSaveCtaToDebDep").val();
        var urlToResumenFinal = $("#UrlToResumenFinal").val();
        GeneralPost(url, obj, function (resp) {
            if (resp.codError === "200") {
                location.href = urlToResumenFinal;
            }

            if (resp.codError === "503")
                alertify.error(resp.mensaje);

        })
    }
}

function ConsultaHostRiesgoGarantias() {
    if (document.getElementById("checkbox").checked == true) {
    var url = $("#UrlToConsultaHost").val();
    var obj = createObjToHost();
    GeneralPost(url, obj, function (resp) {
        if (resp.codError == "200") {
                SolicitarOTPCP();
        }

    })
}
    else {
        alertify.error('Debes autorizar al banco contratar en tu nombre los seguros de Desgravamen y Cesantía por el tiempo del crédito.');
    }
}


function createObjToHost() {
    var obj = {};
    var CodError = "000";
    var MensajeError = " ";
    obj["CodError"] = CodError;
    obj["MensajeError"] = MensajeError;
    return obj;
}


function SolicitarOTPCP() {

    var urlPostSolicitar = $('#UrlSolicitarOTP').val();
    var user = new Object();
    user.cedula = null;
    GeneralPost(urlPostSolicitar, user, function (resp) {
        if (resp.codError == "200")
        {
            document.getElementById("modalCelular").innerHTML = resp.celular;
            bgmodal.showModal("verificacionOTP");
        }
        
    });
}

function ValidarOTPCP() {

    $('#btnOtpFirmar').addClass("bg-loader");

    var urlPostSolicitar = $('#UrlValidarOTP').val();
    var code = $('#code_opt_id').val();
    var newPage = $("#UrlAprobacion").val();

    if (code != "" && code.length === 6) {
        GeneralPost(urlPostSolicitar, { 'data': code }, function (resp) {
            if (resp.codError == "200") {
                window.location.href = newPage;
                $('#btnOtpFirmar').removeClass("bg-loader");
            }
            else {
                alertify.error("Servicio no disponible");
            }
        })
    }
    else {
        alertify.error("Debe ingresar todos los números");
        $('#btnOtpFirmar').removeClass("bg-loader");
    }
}

function ReenviarOTPCP() {

    bgmodal.hideModal('verificacionOTP');
    var text = $('#titulomodal').text();
    $('#titulomodal').text(text.replace('enviamos', 'reenviamos'));
    SolicitarOTPCP();
    blincktext();
}

function blincktextCP() {
    var f = document.getElementById('titulomodal');
    var myVar;
    myVar = setInterval(function () {
        f.style.visibility = (f.style.visibility == 'hidden' ? '' : 'hidden');
    }, 500);

    setTimeout(function () {
        clearInterval(myVar);
        f.style.visibility = '';
    }, 2500);
}

$(document).ready(function () {
    
    $(".bg-primary").removeClass("bg-loader");

    $('#btn_show_modal_id').click(function () {
        bgmodal.showModal('myId');
    });

    $('#btn_hide_modal_id').click(function () {
        bgmodal.hideModal('myId');
    });




    $('#confirmar_soli_id').click(function () {
        if (checkboxpoliticas) {
            SolicitarOTP();
    } else {
            alertify.error('Debe aceptar las polícas de privacidad');
    }
    });


    $('#btn_solicitar_id').click(function () {
        var obj = createObjectForSolicitud();
        if (obj) {
            var urlPostSolicitar = $('#UrlSolicitarPost').val();
            var urlDetalleSolicitud = $('#UrlDetalleSolicitud').val();
            GeneralPost(urlPostSolicitar, obj, function (resp) {
                    location.href = urlDetalleSolicitud;
        });
    }
    });

    var checkboxpoliticas = false;
    $('#aceptar_politicas_id').click(function () {
        checkboxpoliticas = !checkboxpoliticas;
    });

    $('#fcplazo').keydown(function () {
        return false;
    });

    $('#firmar_otp_id').click(ValidarOTP);

    $('#reenviarOTP_id').click(ReenviarOTP);



        ////////////------------  scripts para credito pyme ----------- 
    $('#idModoPago').on('change', OnChangeModoPago);

    $('#idDestinoCredito').on('change', OnChangeDestinoCredito);


    $("#btnCalcularCP").click(CalcularCreditoPyme);

    $("#idSelectDom").change(function () {
        var val = $(this).val();
        
        if (val === "T" || val === "N") {
            $("#idCampoDomHide").show();
        } else {
            $("#idCampoDomHide").hide();
    }
    });

    $("#idTelefonoDom").keyup(function () {
        $(this).val($(this).val().replace(/[^\d,]/g, ''));
    });

        //$("#idBtnContinueToInfPersRes").click(SendDirDomicilio); 
        $("#idBtnContinueToInfPersRes").click(function () {
         var href = $(this).attr('name');
            PostInfoDomAndNeg(false, href);
    });

    $("#idBtnContinueToNegCert").click(function () {
        var href = $(this).attr('name');
        PostInfoDomAndNeg(true, href);
    });

        //$("#idBtnContinueToDom").click(SendEstadoCivil);

        $("#idBtnContinueToClientes").click(SendVentas);

        $("#idBtnContinueToNegocio").click(SendClientesProveedores);

        $("#documento1").change(PostDocument);

        $("#documento2").change(PostDocument);

        $("#documento3").change(PostDocument);

        $("#idBtnFinSubida").click(GenerarVariacionesFE);


        /*change(function () {
        
        var urlToPost = $("#urlToPostFiles").val();
        var file = this.files[0];
        console.log(file.type);
        console.log(file);
        if (file.type != 'application/pdf') {
            alertify.error('solo se admiten formatos con extención .pdf')
            return false;
        }
        var data = new FormData();
        data.append("file0", file);
        console.log(data);
        GeneralPost(urlToPost, data, function (resp) {
                console.log(resp);
            })
    });*/












        /*change(function (e) {
        var files = e.target.files;
        var urlToPost = $("#urlToPostFiles").val();
        if (files.length > 0) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                $.ajax({
                    type: "POST",
                    url: urlToPost,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (result) {
                        if (result) {
                            $("#msg").text("File uploaded successfully!");
                        }
                    },
                    error: function (xhr, status, p3, p4) {
                        var err = "Error " + " " + status + " " + p3 + " " + p4;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).Message;
                        console.log(err);
                    }
                });
            } else {
                alert("Something went wrong! Please try again.");
            }
        }

    });*/


        /*change(function () {
        
        var urlToPost = $("#urlToPostFiles").val();
        var file = this.files[0];
        if (file.type != 'application/pdf') {
            alertify.error('solo se admiten formatos con extención .pdf')
            return false;
        }

        GeneralPost(urlToPost, { 'file1' : file }, function (resp) {
            console.log(resp);
        })
        
    });*/

    $("#idSolteroSi").click(function () {
        window.location.href = $("#urlToInfoDomicilio").val();
    });

    $("#idSolteroNo").click(function () {
        $("#idContainerPregunta").hide();
        $("#idContainerSelection").show();
    });

    $("#idBackToQuestion").click(function () {
        $("#idContainerPregunta").show();
        $("#idContainerSelection").hide();
    });

        /*
        $("#idContinuarToEstCivil").click(function () {
            var correo = $("#idSendEmail").val();
            var urlToPost = $("#UrlPostEmail").val();
            if (validateEmail(correo)) {
                GeneralPost(urlToPost, {"email" : correo}, function (resp) {
                    if (resp.codError === "401")
                        UnAuthorize();
                    if (resp.codError === "400")
                        alertify.error(resp.mensaje);
                    if (resp.codError === "200") {
                        var newUrl = $("#UrlToRedirectNew").val();
                        window.location.href = newUrl;
                    }
                });
            } else {
                alertify.error("correo no válido");
            }
        });*/

    $("#idSeleccionProvincia").on('change', function () {
        var selectedId = $(this).find("option:selected").val();
        PostSelectedLocation(selectedId, "Ciudad");
    });
    $("#idSeleccionCiudad").on('change', function () {
        var selectedId = $(this).find("option:selected").val();
        PostSelectedLocation(selectedId, "Parroquia");
    });

    $("#idSeleccionTieneCert").on('change', function () {
        var text = $(this).children("option:selected").val();
        if (text == 0) {
            $('#idBoxHideCertificacion').hide();
        } else {
            $('#idBoxHideCertificacion').show();
        }
    });

    $("#iso-12188").on('change', function () {
        if ($(this).is(':checked')) {
            $('#idFielDetailCert').show();
        } else {
            $('#idFielDetailCert').hide();
        }
    });

    $('#idContinueToNegResumen').click(SendCertificaciones);

    $('#idContinuarCuentas').click(CreateCbxModalCta);

    $("#idBtnSelectCtasBg").click(SendCtasSelectBg);

    $("#btnFirmar").click(ConsultaHostRiesgoGarantias);

    $("#btnOtpFirmar").click(ValidarOTPCP);

    $('#btnOtpReenviar').click(ReenviarOTPCP);


});