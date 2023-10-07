extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var inventory = get_node("/root/inv")
	$Inventory/H_label.text = str(inventory.hadamard) + " / 5"
	$Inventory/X_label.text = str(inventory.paulix) + " / 5"
	$Inventory/Y_label.text = str(inventory.pauliy) + " / 5"
	$Inventory/Z_label.text = str(inventory.pauliz) + " / 5"
	$Inventory/P_label.text = str(inventory.p) + " / 5"
	$Inventory/CX_label.text = str(inventory.cnot) + " / 5"
	$Inventory/CZ_label.text = str(inventory.cz) + " / 5"
	$Inventory/SW_label.text = str(inventory.swap) + " / 5"
	if Input.is_action_just_pressed("ui_filedialog_show_hidden"):
		if (!$Inventory.visible):
			$Inventory.show()
		else:
			$Inventory.hide()


func _on_open_inventory_button_pressed():
	$Inventory.show()
