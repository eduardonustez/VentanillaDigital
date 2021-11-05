var js_idCompareciente = 1;
function anadirComparecientes() {
   
        js_idCompareciente++;
        idNuevoCompareciente = "compareciente-" + js_idCompareciente;
    $('#comparecientes').append('<div id="nuevoCompareciente' + js_idCompareciente + '" class="mt-3" >' +
            '<div class="align-items-center">' +
            '<div>' +
            '<label class="sr-only" for="numDocumentoCompareciente">Nuevo Compareciente</label>' +
            '<div class="input-group">' +
        '<select name="tipoDocumentoCompareciente' + js_idCompareciente + '" id="tipoDocumentoCompareciente ' + js_idCompareciente +'" class="form-control col-2">' +
            '<option value="0" selected disabled></option>' +
            '<option value="1">C.C</option>' +
            '<option value="2">C.E</option>' +
            '<option value="3">T.I</option>' +
            '<option value="4">PAS</option>' +
            '</select>' +
        '<input type="text" class="form-control col-9" id="numDocumentoCompareciente' + js_idCompareciente +'" placeholder="Número de documento" />' +
            '<button type="button" name="remove" id="' + js_idCompareciente + '" class="ml-2 btn btn-eliminar"><i class="far fa-trash-alt fa-lg"></i></button>' +
            '</div>' +
            '</div>' +
            '<div id="' + idNuevoCompareciente + '"></div>'
        )

        $(document).on('click', '.btn-eliminar', function () {
            var id = $(this).attr('id');
            $('#nuevoCompareciente' + id).remove();
        })
}