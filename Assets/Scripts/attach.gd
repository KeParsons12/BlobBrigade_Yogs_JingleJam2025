extends Area2D

# the attach radius hit box
#@export var attach_hitbox: CollisionShape2D
#@export var attach_hitbox_radius := 2

# limit the amount of objects that can attach
@export var max_attached := 5
@export var attached_objects := []

func _ready() -> void:
	# connect the signal
	area_entered.connect(_on_area_entered)

func _on_area_entered(area: Area2D):
	#print("Touched an area object " + area.name)
	if area.is_in_group("attachable"):
		#print("object touched was in attachable group...")
		call_deferred("attach_object", area)

func attach_object(obj: Node) -> void:
	if obj in attached_objects:
		#print("object is already attached")
		return
	if attached_objects.size() >= max_attached: # if too many objects are attached already
		#print("too many objects attached")
		return
	
	# mark as attached
	attached_objects.append(obj)
	
	# get rid of the objs attachable node
	obj.queue_free()
	# parent the whole object to the player
	obj.get_parent().reparent(self)
	
	#print("attaching the object " + obj.name)
	#print("Number of attached objects " + str(attached_objects.size()))

func detach_object(obj: Node) -> void:
	if obj in attached_objects:
		# unparent back to the scene root (or world container)
		remove_child(obj)
		get_tree().get_current_scene().add_child(obj)
		
		# erase the object from list
		attached_objects.erase(obj)
		var attachable_node = obj.get_node_or_null("Attachable")
		if attachable_node:
			attachable_node.attachable = true
		
		print("unattaching object")
