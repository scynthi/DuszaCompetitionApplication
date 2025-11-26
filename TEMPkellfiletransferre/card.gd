class_name Card extends Control

var mouse_in : bool = false
var is_dragging : bool = false

@onready var sprite_2d: Sprite2D = $Sprite2D
@onready var shadow: Sprite2D = $Sprite2D/Shadow

func _physics_process(delta: float) -> void:
	drag_logic(delta)

var origin_container : Control
func drag_logic(delta : float):
	shadow.position = Vector2(-12, 12).rotated(sprite_2d.rotation)
	
	if (mouse_in or is_dragging) and (MouseBrain.node_being_dragged == null or MouseBrain.node_being_dragged == self):
		if Input.is_action_pressed("click"):
			global_position = lerp(global_position, get_global_mouse_position() - (size / 2.0), 22.0 * delta)
			_change_scale(Vector2(0.85, 0.85))
			sprite_2d.z_index = 100
			
			mouse_filter = Control.MOUSE_FILTER_IGNORE

			is_dragging = true
			MouseBrain.node_being_dragged = self
			
		else:
			_change_scale(Vector2(0.8, 0.8))
			is_dragging = false
			if MouseBrain.node_being_dragged == self:
				MouseBrain.node_being_dragged = null
				
				var hover_control = get_viewport().gui_get_hovered_control()
				var conatiner : Control = hover_control.get_parent()
				if hover_control and conatiner == self.get_parent():
					print("same container", conatiner.name)
					
					var insert_index = _calculate_insert_index(conatiner, hover_control)
					reparent(conatiner)
					conatiner.move_child(self, insert_index)
					mouse_filter = Control.MOUSE_FILTER_STOP
		return
	
	sprite_2d.z_index = 0
	_change_scale(Vector2(0.73, 0.73))
	


func _on_mouse_entered() -> void:
	mouse_in = true
func _on_mouse_exited() -> void:
	mouse_in = false
	
	
var current_goal_scale: Vector2 = Vector2(0.73,0.73)
var scale_tween : Tween
func _change_scale(desired_scale : Vector2):
	if desired_scale == current_goal_scale:
		return
	
	if scale_tween:
		scale_tween.kill()
	scale_tween = create_tween().set_ease(Tween.EASE_OUT).set_trans(Tween.TRANS_BACK)
	scale_tween.tween_property(sprite_2d, "scale", desired_scale, 0.125)
	
	current_goal_scale = desired_scale
	
var last_pos : Vector2
var max_card_rotation : float  = 12.5
func _set_rotation(delta : float):
	var desired_rotation : float = clamp((global_position - last_pos).x * 0.85, -max_card_rotation,max_card_rotation)
	sprite_2d.rotation_degrees = lerp(sprite_2d.rotation_degrees, desired_rotation, 12.0 * delta)
	
	last_pos = global_position


func _calculate_insert_index(container: Control, child_element : Control) -> int:
	var index = container.get_children().find(child_element)
	return index
