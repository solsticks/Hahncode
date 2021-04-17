import { Aurelia } from "aurelia-framework";
import * as environment from "../config/environment.json";
import { I18N, Backend, TCustomAttribute } from "aurelia-i18n";
import { PLATFORM } from "aurelia-pal";
import "whatwg-fetch";
export function configure(aurelia: Aurelia): void {
  aurelia.use
    .standardConfiguration()
    .feature(PLATFORM.moduleName("resources/index"))
    .plugin(PLATFORM.moduleName("aurelia-validation"))
    .plugin(PLATFORM.moduleName("aurelia-dialog"));

  aurelia.use.plugin(PLATFORM.moduleName("aurelia-i18n"), (instance: I18N) => {
    const aliases = ["t", "i18n"];

    TCustomAttribute.configureAliases(aliases);

    instance.i18next.use(Backend.with(aurelia.loader));

    return instance.setup({
      backend: {
        loadPath: "locales/{{lng}}/{{ns}}.json",
      },
      attributes: aliases,
      fallbackLng: "de",
      debug: false,
    });
  });
  aurelia.use.developmentLogging(environment.debug ? "debug" : "warn");

  if (environment.testing) {
    aurelia.use.plugin(PLATFORM.moduleName("aurelia-testing"));
  }

  aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName("app")));
}
