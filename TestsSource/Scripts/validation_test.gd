class_name validation_test
extends GdUnitTestSuite
@warning_ignore('unused_parameter')

#Note this is not really a scalable way of doing validation test, BUT, it is a way that executes fast in the editor,
#only going through the assets once and doing the difference tests in only the set of objects I need to.
#If I could do this in C# I would know to use some interface to make this better, but for now we roll with this system
#This also shouldnt be that bad as long as I dont have too many tests.

const base_project_test_path = "res://CoreTools"

var test_data := [];
var testable_resource_list := [];

func _init() -> void:
	collect_assets(base_project_test_path)
	for resource in testable_resource_list:
		test_data.append([resource.resource_path.split("/", false)[-1], resource])

func collect_assets(inspection_path: String) -> void:
	print("collecting assets at " + inspection_path)
	var dir := DirAccess.open(inspection_path)
	if dir:
		dir.list_dir_begin()
		var file_name := dir.get_next()
		while file_name != "":
			if not dir.current_is_dir():
				print("TESTING")
				print(file_name)
				print(inspection_path + file_name)
				if ResourceLoader.exists(inspection_path + file_name):
					var asset = ResourceLoader.load(inspection_path + file_name, "Resource", ResourceLoader.CacheMode.CACHE_MODE_IGNORE)
					#if we need to validate other type of assets, we should here add them to different categories for validation
					#right now only need resources, so it is fine
					if asset is Resource:
						add_resource_to_test_list_if_needed(asset)
					else:
						#only keep loaded assets we will use for testing
						asset.free()
			else:
				collect_assets(dir.get_current_dir() + "/" + file_name + "/")
					
			file_name = dir.get_next()
	else:
		print("An error occurred when trying to access the path " + inspection_path)


#Here we check all the types we want to test. As I said, not nicely scalable, but with GDScript I am out of options me think
func add_resource_to_test_list_if_needed(resource: Resource) -> void:
	if resource is PoolableObjectReference:
		testable_resource_list.append(resource)
		return


# Here we put all the different validation tests we need to. They should have the same format as the one below.
func test_object_reference_tag(resource_name: String, resource: Resource, test_parameters := test_data) -> void:
	if resource is PoolableObjectReference:
		var object_reference = resource as PoolableObjectReference
		var referenced_object = load(object_reference.Object.resource_path).instantiate()
		assert_str(object_reference.Tag).append_failure_message("Mismatching object pooler tag for poolable reference:" + object_reference.resource_path).is_equal(referenced_object.TagObject)
		referenced_object.free()