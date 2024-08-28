import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { AboutNavMenuComponent } from './components/about/aboutnavmenu/aboutnavmenu.component';
import { HomeComponent } from './components/home/home.component';
import { TermsComponent } from './components/about/terms/terms.component';
import { HowToComponent } from './components/howto/howto.component';
import { PrivacyComponent } from './components/about/privacy/privacy.component';
import { AboutComponent } from './components/about/about.component';
import { StatisticsComponent } from './components/about/statistics/statistics.component';
import { FeedbackComponent } from './components/feedback/feedback.component';
import { DownloadComponent } from './components/download/download.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        AboutNavMenuComponent,
        DownloadComponent,
        HowToComponent,
        FeedbackComponent,
        HomeComponent,
        TermsComponent,
        StatisticsComponent,
        PrivacyComponent,
        AboutComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'howto', component: HowToComponent },
            { path: 'about', component: AboutComponent },
            { path: 'about', component: AboutNavMenuComponent },
            { path: 'about/statistics', component: StatisticsComponent },
            { path: 'downloads', component: DownloadComponent },
            { path: 'feedbacks', component: FeedbackComponent },
            { path: 'about/terms', component: TermsComponent },
            { path: 'about/privacypolicy', component: PrivacyComponent },
            { path: '**', redirectTo: 'home' }
        ]),
    ]
})
export class AppModuleShared {

}
