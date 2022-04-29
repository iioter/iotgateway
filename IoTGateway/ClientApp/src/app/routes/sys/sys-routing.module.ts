import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SysLogComponent } from './log/log.component';

const routes: Routes = [{ path: 'log', component: SysLogComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SysRoutingModule {}
