# StingApp

Responsive website to monitor as many structures as you want.

You'll need node.js and npm, then install angular using the command 'npm install -g @angular/cli'. You have to do these steps to build the angular project.

## Configuring your own Buildings
To configure your own buildings you will need to edit the building.ts file which is saved in the directory StingApp/Source/app. The file already contains an example configuration.
Excerpt from the buildings.ts file:<br><br>
![Node.js](buildings.png)

To configure your own building you can just edit one of the already existing example objects or create a new one following the same schema. The Parameters you will need to provide are:
- name: the name of your building/floor/room
- link: A link to a picture you want to use to represent the building/floor/room
- id: An identifier for your building/floor/room (doesn't necessarily have to be unique)
- room-id: Typically the room number which will be displayed for each room
- x/y: Values that correspond to the location of the status icons on the floor map which indicate the room status. These values must be given in percent (0-100). A x and y value correspond to the icon being at the top-left of the floor map. The x value moves the icon to the right, while the y value moves it towards the bottom.
- device: The name of the measuring device (must be the same as configured in your IoT Hub)
- thresholds: sets the threshold for the room in terms of temperature and humidity

The other values are not yet in use and can be left empty. You can expand this structure to your liking to expand app functionality.

# For Development Purposes
## Development server
Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Build
Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.
