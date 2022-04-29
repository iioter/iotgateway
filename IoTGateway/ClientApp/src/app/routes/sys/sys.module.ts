import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';

import { SysLogComponent } from './log/log.component';
import { SysRoutingModule } from './sys-routing.module';

const COMPONENTS: Array<Type<void>> = [SysLogComponent];

@NgModule({
  imports: [SharedModule, SysRoutingModule],
  declarations: COMPONENTS
})
export class SysModule {}
