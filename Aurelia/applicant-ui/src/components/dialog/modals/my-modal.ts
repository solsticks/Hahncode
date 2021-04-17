import { inject } from "aurelia-framework";
import { DialogController } from "aurelia-dialog";

interface ModelAttributes {
  title: string;
  errors: string[];
}
@inject(DialogController)
export class Prompt {
  controller: DialogController;
  title!: string;
  messages!: ModelAttributes;
  constructor(controller: DialogController) {
    this.controller = controller;
    controller.settings.centerHorizontalOnly = true;
  }
  activate(messages: ModelAttributes): void {
    this.messages = messages;
  }
}
