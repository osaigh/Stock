import { Component, computed, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectSidebarStatus } from '../../statemanagement/sidebar.selectors';

const MOBILE_MAX_WIDTH = 767;

@Component({
  imports: [RouterLink],
  selector: 'app-sidebar',
  styleUrl: './sidebar.css',
  templateUrl: './sidebar.html',
})
export class Sidebar {
  private readonly status = inject(Store).selectSignal(selectSidebarStatus);

  protected readonly classList = computed(() => {
    const status = this.status();
    if (status === '') {
      return { sidebar: 'sidebar', heading: 'title', brand: 'brand', span: '' };
    }
    if (window.innerWidth <= MOBILE_MAX_WIDTH) {
      return { sidebar: 'sidebar show', heading: 'title', brand: 'brand', span: '' };
    }
    return {
      sidebar: `sidebar ${status}`,
      heading: 'title hideheading',
      brand: 'brand hideheading',
      span: 'hidespan',
    };
  });
}
