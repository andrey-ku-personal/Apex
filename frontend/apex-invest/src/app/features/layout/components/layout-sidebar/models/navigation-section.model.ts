import { NavigationItem } from './navigation-item.model';

export interface NavigationSection {
  [sectionName: string]: NavigationItem[];
}