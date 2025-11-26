extends Control

@export var card : PackedScene
@export var container : HBoxContainer

func _on_button_pressed() -> void:
	var new : Card = card.instantiate()
	container.add_child(new)
