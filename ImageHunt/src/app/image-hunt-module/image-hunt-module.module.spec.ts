import { ImageHuntModuleModule } from './image-hunt-module.module';

describe('ImageHuntModuleModule', () => {
  let imageHuntModuleModule: ImageHuntModuleModule;

  beforeEach(() => {
    imageHuntModuleModule = new ImageHuntModuleModule();
  });

  it('should create an instance', () => {
    expect(imageHuntModuleModule).toBeTruthy();
  });
});
