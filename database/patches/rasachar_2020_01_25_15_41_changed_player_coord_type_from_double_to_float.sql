ALTER TABLE `rasachar`.`character`
	ALTER `coord_x` DROP DEFAULT,
	ALTER `coord_y` DROP DEFAULT,
	ALTER `coord_z` DROP DEFAULT,
	ALTER `rotation` DROP DEFAULT;
	
ALTER TABLE `rasachar`.`character`
	CHANGE COLUMN `coord_x` `coord_x` FLOAT NOT NULL AFTER `map_context_id`,
	CHANGE COLUMN `coord_y` `coord_y` FLOAT NOT NULL AFTER `coord_x`,
	CHANGE COLUMN `coord_z` `coord_z` FLOAT NOT NULL AFTER `coord_y`,
	CHANGE COLUMN `rotation` `rotation` FLOAT NOT NULL AFTER `coord_z`;
	
INSERT INTO `rasachar`.`applied_patches` (`patch_name`) VALUES ('rasachar_2020_01_25_15_41_changed_player_coord_type_from_double_to_float');
