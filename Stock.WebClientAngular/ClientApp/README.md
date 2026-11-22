# Stock.WebClientAngular

Angular client of the Stock demo system. It signs the user in against the identity server (OpenID Connect authorization code flow with PKCE, via `oidc-client-ts`) and shows the stock prices and price history from the Stock API.

The identity server, client and API settings are in `src/app/configuration/config.ts`.

## Development server

To start a local development server, run:

```bash
npm start
```

Once the server is running, open your browser and navigate to `http://localhost:44902/`. The port is fixed (see `angular.json`) because that origin is registered with the identity server. The application will automatically reload whenever you modify any of the source files.

## Building

To build the project run:

```bash
npm run build
```

This will compile your project and store the build artifacts in the `dist/` directory (`dist/index.html`). By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Vitest](https://vitest.dev/) test runner, use the following command:

```bash
npm test
```

Use `npm test -- --watch=false` for a single run.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
