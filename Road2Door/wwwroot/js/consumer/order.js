
let itemId;
let menuId;

$(document).ready(function () {
    var maxQuantity = parseInt($("#quantity").val());
    document.getElementById('quantity').max = maxQuantity;
    // Add click event listener to edit buttons
    var maxQuantity = parseInt($("#quantity").val());
    document.getElementById('quantity').max = maxQuantity;
    $('.edit-btn').click(function () {

        var itemName = $(this).closest("tr").find("td:nth-child(2)").text();
        var itemQuantity = $(this).closest("tr").find("td:nth-child(4)").text();
        var itemId = $(this).data('item-id');
        var menuId = $(this).data('menu-id');
        // Update modal content with item details
        $("#modal-item-id").text(itemId);
        $("#modal-menu-id").text(menuId);
        $("#modal-item-name").text(itemName);
        $("#modal-item-quantity").text(itemQuantity);

        $("#itemId").val(itemId);
        $("#menuId").val(menuId);

        // Set the maximum limit for the quantity input field
        $("#quantity").attr("max", itemQuantity);

        // Show modal
        $('#edit-modal').css('display', 'block');
        $('.close').click(function () {
            $('#edit-modal').css('display', 'none');
        });
    });
    $('#add-orderItem').click(function () {

        // Get the entered quantity
        var quantity = parseInt($("#quantity").val());
        console.log(quantity + itemId + menuId);
        // Get the item's quantity from the inventory table
        var itemQuantity = $("#modal-item-quantity").text(itemQuantity);
        var data = {
            itemId: itemId,
            quantity: quantity,
            menuId: menuId
        };
        $.ajax({
            url: "/Consumer/AddToCart",
            type: "POST",
            data: data,
            success: function (response) {
                // Handle the success response
                // (e.g., display a success message, update the UI, etc.)
                console.log(response);
                location.reload();
            },
            error: function (xhr, status, error) {
                // Handle the error response
                // (e.g., display an error message, handle specific error cases, etc.)
                console.error(error);
            }
        });

    });

    $('.IncreaseDecreaseOrderItem').click(function () {
        console.log("in the increase decrease");
        var itemId = $(this).data('item-id');
        var menuId = $(this).data('menu-id');

        console.log("itemId" + itemId);
        console.log("menuId" + menuId);

        var itemName = $(".edit-btn[data-item-id='" + itemId + "']").closest("tr").find("td:nth-child(2)").text();
        var menuitemQuantity = $(".edit-btn[data-item-id='" + itemId + "']").closest("tr").find("td:nth-child(4)").text();
        var itemQuantityOfOrder = $(this).closest("tr").find("td:nth-child(2)").text();
        console.log(itemId + " " + itemName + " " + menuitemQuantity)
        // Update modal content with item details
        $("#modal-order-itemid").text(itemId);
        $("#modal-order-itemname").text(itemName);
        $("#modal-order-menuitemquantity").text(menuitemQuantity);
        $("#modal-order-itemquantity").text(itemQuantityOfOrder);
        $("#modal-order-quantity").text(itemQuantityOfOrder);
        var updatedVal = parseInt(itemQuantityOfOrder);
        var count = 0;

        // Plus button click event
        $(document).off("click", ".plus-btn").on("click", ".plus-btn", function () {
            if (count < parseInt(menuitemQuantity) || updatedVal < parseInt(itemQuantityOfOrder)) {
                updatedVal = updatedVal + 1;
                count = count + 1;

                $("#modal-order-quantity").text(updatedVal.toString()); // Update displayed quantity
            }
            if (count > parseInt(itemQuantityOfOrder)) {
                $(".plus-btn").prop("disabled", true); // Disable plus button
            }
        });

        // Minus button click event
        $(document).off("click", ".minus-btn").on("click", ".minus-btn", function () {
            if (updatedVal > 0) {
                updatedVal = updatedVal - 1;
                count = count - 1;

                $("#modal-order-quantity").text(updatedVal.toString());
                $(".plus-btn").prop("disabled", false);
            }
        });

        $('#edit-order-modal').css('display', 'block');

        // Click event for close button
        $('.close').click(function () {
            $('#edit-order-modal').css('display', 'none');
        });

        // Function to submit the form and update the item quantity in the menu
        $(document).off("submit", "#update-menu").on("submit", "#update-menu", function (e) {
            e.preventDefault();
            console.log("inside update button click function2");

            var updatedQuantity = updatedVal.toString(); // Convert updatedVal to string

            $("#modal-order-itemid").text(itemId);
            $("#modal-order-itemquantity").text(menuitemQuantity);
            $("#modal-order-itemquantity").text(itemQuantityOfOrder);
            $("#modal-order-quantity").text(itemQuantityOfOrder);
            console.log(itemId);
            console.log(menuitemQuantity);
            console.log(itemQuantityOfOrder);
            console.log(updatedQuantity);
            var data = {
                itemId: itemId,
                orderItemQuantity: itemQuantityOfOrder,
                updatedQuantity: updatedQuantity,
                menuId: menuId
                // Pass updatedQuantity instead of updatedVal
            };

            // Make an AJAX request to update the item quantity
            $.ajax({
                url: "/Consumer/UpdateOrderItemQuantity",
                method: "post",
                data: data,
                success: function (response) {
                    count = 0;
                    // Handle the success response
                    // (e.g., display a success message, update the UI, etc.)
                    $('#edit-order-modal').css('display', 'none');

                    var updatedMenuQuantity = response.updatedMenuQuantity;
                    var updatedOrderQuantity = response.updatedOrderQuantity;
                    console.log("Menu is " + updatedMenuQuantity + "order is " + updatedOrderQuantity)

                    // Update the inventory table with the new quantity
                    $(".edit-btn[data-item-id='" + itemId + "']").closest("tr").find("td:nth-child(4)").text(updatedMenuQuantity);

                    // Update the menu table with the new quantity
                    $(".IncreaseDecreaseOrderItem[data-item-id='" + itemId + "']").closest("tr").find("td:nth-child(2)").text(updatedOrderQuantity);
                    location.reload();

                    console.log("success");
                },
                error: function (xhr, status, error) {
                    // Handle the error response
                    // (e.g., display an error message, handle specific error cases, etc.)
                    console.error(error);
                }
            });
        });
    });

});