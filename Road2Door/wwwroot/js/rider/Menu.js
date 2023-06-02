

let itemId;
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
        // Update modal content with item details
        $("#modal-item-id").text(itemId);
        $("#modal-item-name").text(itemName);
        $("#modal-item-quantity").text(itemQuantity);

        $("#itemId").val(itemId);

        // Set the maximum limit for the quantity input field
        $("#quantity").attr("max", itemQuantity);

        // Show modal
        $('#edit-modal').css('display', 'block');
        $('.close').click(function () {
            $('#edit-modal').css('display', 'none');
        });
    });
    $('#add-menuItem').click(function () {

        // Get the entered quantity
        var quantity = parseInt($("#quantity").val());

        // Get the item's quantity from the inventory table
        var itemQuantity = $("#modal-item-quantity").text(itemQuantity);
        var data = {
            itemId: itemId,
            quantity: quantity,
        };
        $.ajax({
            url: "/Rider/AddToMenu",
            type: "POST",
            data: data,
            success: function (response) {
                // Handle the success response
                // (e.g., display a success message, update the UI, etc.)
                console.log(response);
            },
            error: function (xhr, status, error) {
                // Handle the error response
                // (e.g., display an error message, handle specific error cases, etc.)
                console.error(error);
            }
        });

    });

    $('.IncreaseDecreaseMenuItem').click(function () {

        var itemId = $(this).data('item-id');
        var itemName = $(".edit-btn[data-item-id='" + itemId + "']").closest("tr").find("td:nth-child(2)").text();
        var itemQuantity = $(".edit-btn[data-item-id='" + itemId + "']").closest("tr").find("td:nth-child(4)").text();
        var itemQuantityOfMenu = $(this).closest("tr").find("td:nth-child(2)").text();
        // Update modal content with item details
        $("#modal-itemid").text(itemId);
        $("#modal-itemname").text(itemName);
        $("#modal-itemquantity").text(itemQuantity);
        $("#modal-menu-itemquantity").text(itemQuantityOfMenu);
        $("#modal-menu-quantity").text(itemQuantityOfMenu);
        var updatedVal = parseInt(itemQuantityOfMenu);
        var count = 0;

        $(".plus-btn").click(function () {

            if (parseInt(count) < itemQuantity) {
                updatedVal = updatedVal + 1;
                count = count + 1;
                console.log("here is the count" + count);

                $("#modal-menu-quantity").text(updatedVal.toString()); // Update displayed quantity

            }


            if (count > itemQuantityOfMenu) {
                $(".plus-btn").prop("disabled", true); // Disable plus button
            }
        });
        // Minus button click event
        $(".minus-btn").click(function () {
            if (updatedVal > 0) {
                updatedVal = updatedVal - 1;
                console.log(typeof (updatedVal));
                $("#modal-menu-quantity").text(updatedVal.toString());
            }
        });

        $('#edit-menu-modal').css('display', 'block');

        // Click event for close button
        $('.close').click(function () {
            $('#edit-menu-modal').css('display', 'none');
        });
        // Function to submit the form and update the item quantity in the menu
        $(document).on("submit", "#update-menu", function (e) {
            console.log("inside update button click function2");

            e.preventDefault();
            console.log(itemId);
            console.log(itemQuantity);
            console.log(itemQuantityOfMenu);
            console.log(updatedVal);
            var data = {
                itemId: itemId,
                menuItemQuantity: itemQuantityOfMenu,
                updatedQuantity: updatedVal
            };
            // Make an AJAX request to update the item quantity
            $.ajax({
                url: "/Rider/UpdateMenuItemQuantity",
                method: "post",
                data: data,
                success: function (response) {
                    // Handle the success response
                    // (e.g., display a success message, update the UI, etc.)
                    $('#edit-menu-modal').css('display', 'none');

                    var updatedInventoryQuantity = response.updatedInventoryQuantity;
                    var updatedMenuQuantity = response.updatedMenuQuantity;

                    // Update the inventory table with the new quantity
                    $(".edit-btn[data-item-id='" + itemId + "']").closest("tr").find("td:nth-child(4)").text(updatedInventoryQuantity);

                    // Update the menu table with the new quantity
                    $(".IncreaseDecreaseMenuItem[data-item-id='" + itemId + "']").closest("tr").find("td:nth-child(2)").text(updatedMenuQuantity);


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

function deleteItem(itemId, quantity) {

    if (confirm("Are you sure you want to delete this item from the menu?")) {
        var url = "/Rider/DeleteItemFromMenu?itemId=" + itemId + "&quantity=" + quantity;
        window.location.href = url;
    }
}

