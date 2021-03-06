const { dest, pipe, src } = require('gulp')
const concat = require('gulp-concat')

const buildMigrations = () =>
  src('./Meetups.Persistence/Migrations/*.sql')
    .pipe(concat('migrations.sql', { newLine: '\n\n' }))
    .pipe(dest('./'))

exports.buildMigrations = buildMigrations
