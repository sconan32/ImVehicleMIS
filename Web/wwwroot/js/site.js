﻿//                XXXXXX
//           XXXXXXXXXXXXX X
//      XXXXXXXXXXXXXXXXXXXXXXXX
//      XXXXXXXXXXX       XXXXXXX
//      XXXXXXXXX            XXXXX
//      X XXXXXX              XXXXX
//       XXXXXXX                XX
//       XXXXXXX
//          XXXXX
//            XXXXX
//               XXXX
//                  XXXX
//                    XXXXXX                                                                                                                  XXXXXXX
//                       XXXXXX                   XXXXXX              XX XXX             XXX XXXXX             XX        XXXX            X XXXXXXXX XXX
//                         XXX XXX             XXXXXXXXXXX        X XXXXXXXXXXX         XXXXXXXXXXXX X       XXXXX     XXXXXXXX         XXXXXX    XXXXXX
//                          XXX XXXXX         XXXXXX X   XXX  X    XXXXXXXX  XXXX     XXXXXXXX    XXXXX      XXXXXX    XXXXXXXX         X         XXXXXXX
//XX                         XXXXXXXXX      XXXXXXXXXXXXXXXXX     XXXXXXXX     XX     XXXXXXXXX     XXX    X  X XXX XXX   X XXXX       XX          XXXXXXX
// XX                         XXX XXXXX     XX XX         XX X    XXXXXX             XXXXX    XXX X  XX X       XX XXX     XXXXX      XX         XXXXXXXXX
//  XX                        XXXXXXXXX     XXXX           XXX   XXXXXX              XXXX            XXX       XXXXX       XXXXX     X          XXXX XXXXXX
//   XX                      XXXXXXXXXX    XXX              XX   XXXX                 XXX           XXXX      XXXXX       XXXXX     X       XXXXXXXXXXXXXX
//   XXXXX                XXXXXXXXXXX      XXX            XXXX   XXXXX              XXXXX         XXXXX      XXXXXX       XXXXX   XXXX    XXXXXX     XXXXX
//     XXXXXX XXXXXXXXXXXXXX XXXXXXXX      XXX          XXXXXX   XXXXX            XXX  XX         XXXXX      XXXXX        XXXX    XX      XXXXX       XXXXX      XX
//       XXXXXXXXXXXXXXXXXXXXXXXXXX         XX        XXXXXXX      XXXXXX     XXXXX     XXX  XXXXXXXXXX     XXXXX         XXXX  XXX      XXXX       XXXXXXX     XX
//          XXXXXXXXXXXXXXXXXXXXX            XXXX  XXXXXXXXX       XXXXXXXXXXXX          XXXXXXXXXXXXX     XXXXX          XXXX XXX        XXXX    XXX X XXXX XXXX
//            XXXX XX XXXXXXX                    XXXXXXX             XXXX                   XXXXXXX        XXXX           XXXXXXX         XXXXXXXXX   XXXXXXXXX

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

$(document).ready(function () {
    //Dropdownlist Selectedchange event
    $(document).on('click', 'th', function () {
        var table = $(this).parents('table').eq(0);
        var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()));
        this.asc = !this.asc;
        if (!this.asc) { rows = rows.reverse(); }
        table.children('tbody').empty().html(rows);
    });
    function comparer(index) {
        return function (a, b) {
            var valA = getCellValue(a, index), valB = getCellValue(b, index);
            return $.isNumeric(valA) && $.isNumeric(valB) ?
                valA - valB : valA.localeCompare(valB);
        };
    }
    function getCellValue(row, index) {
        return $(row).children('td').eq(index).text();
    }
});