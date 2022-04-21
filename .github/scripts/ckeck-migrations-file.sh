﻿
MIGRATIONS_SQL_FILE_NAME="migrations.sql"
MIGRATIONS_SQL_FILE_EXISTS=$([ -f "./$MIGRATIONS_SQL_FILE_NAME" ] -eq 1)
GENERATE_MIGRATIONS_SQL_FILE_COMMAND="npm run build-migrations"

echo "Migrations sql file name: $MIGRATIONS_SQL_FILE_NAME"
echo "Migrations sql file exists: $MIGRATIONS_SQL_FILE_EXISTS"
echo "Generate migrations sql file command: $GENERATE_MIGRATIONS_SQL_FILE_COMMAND"

if [[ ! -f "./$MIGRATIONS_SQL_FILE_NAME" ]]; then
    echo "FAILURE: The merged migrations file \"$MIGRATIONS_SQL_FILE_NAME\" is missing."
    echo "HINT: You can generate this file using the \"$GENERATE_MIGRATIONS_SQL_FILE_COMMAND\" command in the root directory of the repository."
    exit 1
fi

echo "Generating new $MIGRATIONS_SQL_FILE_NAME file to check if it matches provided one..."

$GENERATE_MIGRATIONS_SQL_FILE_COMMAND
MIGRATIONS_SQL_MODIFIED=$(git status --porcelain -- "./$MIGRATIONS_SQL_FILE_NAME")

if [[ $MIGRATIONS_SQL_MODIFIED ]]; then
    echo "FAILURE: The provided $MIGRATIONS_SQL_FILE_NAME file must exactly match the automatically generated one."
    exit 1
else
    echo "SUCCESS: Automatically generated $MIGRATIONS_SQL_FILE_NAME file matches the provided one."
fi
